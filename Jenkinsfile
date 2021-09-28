pipeline {
    agent {
        dockerfile {
            filename 'Dockerfile.sdk'
        }
    }
    stages {
        stage('Verify') {
            steps {
                sh '''
                  dotnet --list-sdks
                  dotnet --list-runtimes
                '''
                sh 'printenv'
                sh 'ls -l "$WORKSPACE"'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build "$WORKSPACE/Britannica.Host/Britannica.Host.csproj"'
            }
        }
        stage('Unit Test') {
            steps {
                sh 'dotnet test "$WORKSPACE/Britannica.UnitTest/Britannica.UnitTest.csproj"'
            }
        }
        stage('Smoke Test') {
            steps {
                sh 'dotnet "$WORKSPACE/Britannica.Host/bin/Debug/netcoreapp3.1/Britannica.Host.dll"'
            }
        }
    }
}
