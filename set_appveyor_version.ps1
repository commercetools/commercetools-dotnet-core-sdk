#$env:APPVEYOR_REPO_TAG = "false"
#$env:APPVEYOR_REPO_TAG_NAME = "1.2.0"
#$env:APPVEYOR_BUILD_NUMBER = "1"
#$env:APPVEYOR_PULL_REQUEST_NUMBER = ""
#$env:APPVEYOR_REPO_BRANCH = "dfd f"
#$env:CONFIGURATION = "Release"
#$env:APPVEYOR_BUILD_VERSION = "1.0.0.1"

function getVersion() {
    $version = $env:APPVEYOR_BUILD_VERSION;
    $buildNumber = $env:APPVEYOR_BUILD_NUMBER;
    if ($version -match "^[1-9]+(\.[0-9]+){3}$")
    {
        $versionParts = $version.Split(".");
        $version = $versionParts[0] + "." + $versionParts[1] + "." + $versionParts[2] + "-" + $versionParts[3];
    }
    elseif ($version -match "^[1-9]+(\.[0-9]+){2}$")
    {
        $version = $version + "-" + $buildNumber;
    }

    $dbgSuffix = If ($env:CONFIGURATION -eq "Debug") { "-dbg" } else { "" };

    if ($env:APPVEYOR_REPO_TAG -eq "true")
    {
        if ($env:APPVEYOR_REPO_TAG_NAME -match "^[1-9]+(\.[0-9]+){3}$")
        {
            $versionParts = $env:APPVEYOR_REPO_TAG_NAME.Split(".");
            $mainVersion = $versionParts[0] + "." + $versionParts[1] + "." + $versionParts[2] + "-" + $versionParts[3];
            $packageVersion = $mainVersion + "-" + $buildNumber;
        }
        elseif ($env:APPVEYOR_REPO_TAG_NAME -match "^[1-9]+(\.[0-9]+){2}-")
        {
            $packageVersion = $env:APPVEYOR_REPO_TAG_NAME + "-" + $buildNumber;
        }
        elseif ($env:APPVEYOR_REPO_TAG_NAME -match "^[1-9]+(\.[0-9]+){2}$")
        {
            $packageVersion = $env:APPVEYOR_REPO_TAG_NAME;
        }
        else
        {
            $packageVersion = $version + "-" + $env:APPVEYOR_REPO_TAG_NAME + "-" + $buildNumber;
        }
    }
    else
    {
        $branch = $env:APPVEYOR_REPO_BRANCH;
        $noPR = [string]::IsNullOrEmpty($env:APPVEYOR_PULL_REQUEST_NUMBER);

        if ($branch -eq "master" -And $noPR)
        {
            $packageVersion = $version + "-dev"  + $dbgSuffix;
        }
        else
        {
            $suffix = "-ci" + $dbgSuffix;

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
            if ($suffix.Length -gt 21)
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
