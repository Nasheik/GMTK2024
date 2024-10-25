pipeline {
    agent any
    
    triggers {
        pollSCM '*/5****'
    }

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