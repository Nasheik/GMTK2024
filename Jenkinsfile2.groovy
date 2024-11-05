pipeline {
    agent any

    environment {
        BUILD_NAME = 'Testing'
        UNITY_DIR = "./GMTK2024/"
    }

    stages {
        stage('Checkout Code') {
            steps {
                // Checkout the code from a Git repository
                bat("""
                    git clone --single-branch --branch main https://github.com/Nasheik/GMTK2024.git
                    cd GMTK2024
                    git pull
                    echo "pulled the code"
                """)
            }
        }
        stage('Build and Test') {
            steps {
                // Run before script and test script using PowerShell
                bat '''
                    echo hello
                    GMTK2024/ci/build.bat
                '''
            }
        }
    }
}