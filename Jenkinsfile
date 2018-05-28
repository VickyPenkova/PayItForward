#!groovy

def sonarQubeUrl = 'https://sonarqube.fourth.com'
def sonarQubeProjectKey = "PayItForward"

def isPullRequest() { return env.CHANGE_ID != null }
def onReleaseBranch() {
	return env.BRANCH_NAME == releaseBranchName
}

releaseBranchName = "master"
workspace = "${env.WORKSPACE}/PayItForward/${env.BRANCH_NAME}".replace('%2F', '_')

properties([
    buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '10', daysToKeepStr: '30', numToKeepStr: '')),
    [$class: 'RebuildSettings', autoRebuild: false, rebuildDisabled: false ],
    [$class: 'ThrottleJobProperty', categories: [], limitOneJobWithMatchingParams: false, maxConcurrentPerNode: 0, 
        maxConcurrentTotal: 1, paramsToUseForLimit: '', throttleEnabled: false, throttleOption: 'project' ],
    pipelineTriggers([])
])

try {
    node ('Oblivion') {
        ws(workspace) {
            def sonarScanner = tool name: 'SonarQubeScanner', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
            
            stage('Get source code') {
                checkout([
                    $class: 'GitSCM', 
                    branches: scm.branches,
                    extensions: scm.extensions + [[$class: 'CleanCheckout']],
                    userRemoteConfigs: scm.userRemoteConfigs
                ])
            }

            // Build started
            notifyMessengers()

            stage('Build') {
                sonarCommand =  "${sonarScanner}/SonarQube.Scanner.MSBuild.exe begin \
                    /k:\"${sonarQubeProjectKey}\" \
                    /d:sonar.host.url=\"${sonarQubeUrl}\" \
                    /d:sonar.cs.opencover.reportsPaths=\"opencover-shared.xml\" \
                    /d:sonar.cs.xunit.reportsPaths=\"unittestresult.xml\" \
                    /v:\"${env.BUILD_ID}\" \
                    /d:sonar.language=cs \
                    /d:sonar.verbose=true"

                // In case of pull request do scan with comments only
                if (isPullRequest()) {
                    withCredentials([string(credentialsId: 'PayItForwardGitHub', variable: 'GitHubToken')]) {
                        sonarCommand += " \
                        /d:sonar.analysis.mode=preview \
                        /d:sonar.github.pullRequest=${CHANGE_ID} \
                        /d:sonar.github.repository=fourth/Fourth.Inventory.Project \
                        /d:sonar.github.oauth=${GitHubToken}"
                    }
                }

                bat sonarCommand                

                bat "dotnet restore \"$env.WORKSPACE/PayItForward/PayItForward/PayItForward.sln\""     
                bat "dotnet build --configuration \"Release\""
                // bat "dotnet publish \"$env.WORKSPACE/$pathToAPI\" --configuration \"Release\" --output published-website"
                // bat "dotnet publish \"$env.WORKSPACE/$pathToWEB\" --configuration \"Release\" --output published-website"
            }
            

            stage('Run Unit tests') {
                // Timeout in minutes
                timeout (20) {
                    try {
                        echo 'Run unit tests'
                        dir ('PayItForward/PayItForward/PayItForward.ConsoleClient.UnitTests') {
                            bat "dotnet xunit --fx-version 2.0.5 -xml \"$env.WORKSPACE\\unittestresult.xml\""
                        }
                    } finally {
                        echo 'Publish test results'                   
                        step([$class: 'XUnitBuilder',
                            thresholds: [[$class: 'FailedThreshold', unstableThreshold: '1'],
                                [$class: 'SkippedThreshold', failureNewThreshold: '3', failureThreshold: '5']],
                            tools: [[$class: 'XUnitDotNetTestType', pattern: "unittestresult.xml"]]])

                        echo 'Generate code coverage'
                        bat "\"${OpenCoverConsoleExe}\" \
                            -output:\"opencover-shared.xml\" \
                            -register:user -target:\"dotnet.exe\" \
                            -targetargs:\" \
                                \\\"C:/Users/jenkins/.nuget/packages/xunit.runner.console/2.3.1/tools/netcoreapp2.0/xunit.console.dll\\\" \
                                \\\"$env.WORKSPACE/PayItForward/PayItForward/PayItForward.ConsoleClient.UnitTests/bin/Debug/netcoreapp2.0/PayItForward.ConsoleClient.UnitTests.dll\\\"\" \
                            -oldstyle -skipautoprops -hideskipped:All"

                            // -filter:\"+[Fourth.Inventory.SupportTools*]* -[Fourth.Inventory.SupportTools.Data*]* -[Fourth.Inventory.SupportTools.Models]*\" \
                    }
                }
            }

            stage('Publish SonarQube analysis') {
                echo 'Publish SonarQube analysis'
                bat "${sonarScanner}/SonarQube.Scanner.MSBuild.exe end"
            }
            
            // if(onReleaseBranch()){
            //     stage('Publish') {
            //         bat "del *.nupkg"
            //         echo "Creating package $packageNameAPI"		    	
            //         bat "octo pack --id $packageNameAPI --version \"$buildVersion\" --basePath \"$env.WORKSPACE/Fourth.Inventory.SupportTools.WebApi/published-website\" --format=NuGet"
            //         archiveArtifacts artifacts:"$packageNameAPI*.nupkg", excludes: null

            //         echo "Creating package $packageNameWEB"		    	
            //         bat "octo pack --id $packageNameWEB --version \"$buildVersion\" --basePath \"$env.WORKSPACE/Fourth.Inventory.SupportTools.Web/published-website\" --format=NuGet"
            //         archiveArtifacts artifacts:"$packageNameWEB*.nupkg", excludes: null
                    
            //         withCredentials([string(credentialsId: 'OctopusApiKey', variable: 'OCTOPUS_API_KEY')]) {
            //             echo "Publishing package $packageNameAPI to Octopus"
            //             bat "octo push --replace-existing --package $packageNameAPI.${buildVersion}.nupkg --server $octopusUrl --apiKey $OCTOPUS_API_KEY"   			

            //             echo "Publishing package $packageNameWEB to Octopus"
            //             bat "octo push --replace-existing --package $packageNameWEB.${buildVersion}.nupkg --server $octopusUrl --apiKey $OCTOPUS_API_KEY"   			
                    
            //             echo "Creating project release"
            //             bat "octo create-release --project=\"$octopusSupportToolProjectName\" --version=$buildVersion --ignoreexisting --server $octopusUrl --apiKey $OCTOPUS_API_KEY"
            //         }
            //     }
            // }

            notifyMessengers(currentBuild.result)
        }
    }
}
catch(exception) {
    // If the build is not aborted
    if (!manager.build.getAction(InterruptedBuildAction.class)) {
        currentBuild.result = "FAILURE"
        notifyMessengers(currentBuild.result)
    }

    echo "${exception}"
    throw exception
}

def notifyMessengers(String buildStatus = 'STARTED') {
    // Build status of null means successful.
    buildStatus = buildStatus ?: 'SUCCESS'
    // Replace encoded slashes.
    def decodedJobName = env.JOB_NAME.replaceAll("%2F", "/")

    def colorSlack
    if (buildStatus == 'STARTED') {
        colorSlack = '#D4DADF'
    } else if (buildStatus == 'SUCCESS') {
        colorSlack = '#BDFFC3'
    } else if (buildStatus == 'UNSTABLE') {
        colorSlack = '#FFFE89'
    } else {
        colorSlack = '#FF9FA1'
    }

    def msgPrefix = ''
    if (buildStatus == 'FAILURE' && onReleaseBranch()) {
        msgPrefix = "@channel "
    }

    def msgSlack = "${msgPrefix}${env.BRANCH_NAME}: ${buildStatus}\n`${decodedJobName}` <${env.BUILD_URL}|#${env.BUILD_NUMBER}>"

    slackSend(color: colorSlack, message: msgSlack, channel: '#PayItForward', tokenCredentialId: 'PayItForwardSlackToken')
}
