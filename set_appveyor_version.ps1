#$env:APPVEYOR_REPO_TAG = "false"
#$env:APPVEYOR_REPO_TAG_NAME = "1.2.0"
#$env:APPVEYOR_BUILD_NUMBER = "1"
#$env:APPVEYOR_PULL_REQUEST_NUMBER = ""
#$env:APPVEYOR_REPO_BRANCH = "dfd f"
#$env:CONFIGURATION = "Release"
#$env:APPVEYOR_BUILD_VERSION = "1.0.0.1"

function getVersion() {
    $version = $env:APPVEYOR_BUILD_VERSION.split('.')[0..2] -join '.';
    $dbgSuffix = If ($env:CONFIGURATION -eq "Debug") { "-dbg" } else { "" };
    $buildNumber = $env:APPVEYOR_BUILD_NUMBER;

    if ($env:APPVEYOR_REPO_TAG -eq "true")
    {
        $packageVersion = $env:APPVEYOR_REPO_TAG_NAME + "." + $buildNumber;
    }
    else
    {
        $branch = $env:APPVEYOR_REPO_BRANCH;
        $noPR = [string]::IsNullOrEmpty($env:APPVEYOR_PULL_REQUEST_NUMBER);

        if ($branch -eq "master" -And $noPR)
        {
            $packageVersion = $version + "-beta" + $buildNumber + $dbgSuffix;
        }
        else
        {
            $suffix = "-alpha" + $buildNumber + $dbgSuffix;

            if (!$noPR)
            {
                $suffix += "-pr-" + $env:APPVEYOR_PULL_REQUEST_NUMBER;
            }
            elseif ($branch.StartsWith("release", "CurrentCultureIgnoreCase"))
            {
                $suffix += "-pre";
            }
            else
            {
                $safeBranch = $branch -replace "[^0-9A-Za-z-]+", ""
                $suffix += "-" + $safeBranch;
            }

            # Nuget limits "special version part" to 20 chars. Add one for the hyphen.
            if ($suffix.Length > 21)
            {
                $suffix = $suffix.Substring(0, 21);
            }

            $packageVersion = $version + $suffix;
        }
    }

    return $packageVersion;
}


$version = getVersion
echo "Build version: $version"
Update-AppveyorBuild -Version $version
