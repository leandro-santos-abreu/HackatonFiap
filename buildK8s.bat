kubectl create secret generic certificate-secret --from-file=certificate.pfx=F:/PosTechFiap/https-certificate/certificate.pfx

kubectl apply -f HealthMed.Persistence\\Kubernets\\Database\\PersistentVolume
kubectl apply -f HealthMed.Persistence\\Kubernets\\Database\\PersistentVolumeChain
kubectl apply -f HealthMed.Persistence\\Kubernets\\Database
kubectl apply -f HealthMed.Persistence\\Kubernets\\Api
kubectl apply -f HealthMed.Persistence\\Kubernets\\HealthMed.Agendamentos
kubectl apply -f HealthMed.Persistence\\Kubernets\\HealthMed.Notificacoes


kubectl apply -f HealthMed.Persistence\\Kubernets\\Grafana\\PersistentVolume
kubectl apply -f HealthMed.Persistence\\Kubernets\\Grafana\\PersistentVolumeChain
kubectl apply -f HealthMed.Persistence\\Kubernets\\Grafana\\PersistentVolumeCopy

timeout 10

kubectl cp grafana copy-data-to-pv:/etc

kubectl apply -f HealthMed.Persistence\\Kubernets\\Grafana
kubectl apply -f HealthMed.Persistence\\Kubernets\\NodeExporter
kubectl apply -f HealthMed.Persistence\\Kubernets\\Prometheus
kubectl apply -f HealthMed.Persistence\\Kubernets\\RabbitMQ\\Data\\PersistentVolume
kubectl apply -f HealthMed.Persistence\\Kubernets\\RabbitMQ\\Data\\PersistentVolumeChain
kubectl apply -f HealthMed.Persistence\\Kubernets\\RabbitMQ\\Logs\\PersistentVolume
kubectl apply -f HealthMed.Persistence\\Kubernets\\RabbitMQ\\Logs\\PersistentVolumeChain
kubectl apply -f HealthMed.Persistence\\Kubernets\\RabbitMQ