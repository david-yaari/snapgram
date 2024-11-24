kubectl apply -f configmap.yaml
kubectl apply -f secret.yaml
kubectl apply -f pvc.yaml
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml

kubectl get deployment mongodb-deployment-authentication

kubectl get service mongodb-service-authentication

kubectl describe deployment mongodb-deployment-authentication
kubectl describe service mongodb-service-authentication