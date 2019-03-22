function getVersion() {
    $version = "1.0.0";
    $dbgSuffix = If ($env:CONFIGURATION -eq "Debug") { "-dbg" } else { "" };

    if ($env:APPVEYOR_REPO_TAG -eq "true")
    {
        $packageVersion = $env:APPVEYOR_REPO_TAG_NAME;
    }
    else
    {
        $buildNumber = $env:APPVEYOR_BUILD_NUMBER;
        $branch = $env:APPVEYOR_REPO_BRANCH;
        $noPR = [string]::IsNullOrEmpty($env:APPVEYOR_PULL_REQUEST_NUMBER);

        if ($branch -eq "master")
        {
            $packageVersion = $version + "-dev-" + $buildNumber + $dbgSuffix;
        }
        else
        {
            $suffix = "-ci-" + $buildNumber + $dbgSuffix;

            if (!$noPR)
            {
                $suffix += "-pr-" + $env:APPVEYOR_PULL_REQUEST_NUMBER;
            }
            elseif ($branch.StartsWith("release", "CurrentCultureIgnoreCase"))
            {
                $suffix += "-pre-" + $buildNumber;
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