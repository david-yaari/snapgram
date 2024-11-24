kubectl apply -f configmap.yaml
kubectl apply -f secret.yaml
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml
kubectl apply -f ingress.yaml

kubectl get deployment mongo-express

kubectl get service mongo-express-service

kubectl get pods

kubectl port-forward mongo-express-796656c447-6vwt9 8081:8081

kubectl delete -f deployment.yaml
kubectl delete -f service.yaml