pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                echo "Building.."
                bat ''' 
                echo "Building" 
                '''
            }
        }
        stage('Test') {
            steps {
                echo "Testing.."
            }
        }
        stage('Deliver') {
            steps {
                echo 'Deliver....'
            }
        }
    }
}