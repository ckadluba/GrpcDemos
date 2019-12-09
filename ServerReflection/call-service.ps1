Write-Host "**************** Server reflection list **********************"
grpcurl -v -plaintext localhost:5001 list
Write-Host "--- Press any key ---"
Read-Host

Write-Host "**************** Server reflection describe ******************"
grpcurl -v -plaintext localhost:5001 describe
Write-Host "--- Press any key ---"
Read-Host

Write-Host "**************** Calling service *****************************"
grpcurl -v -plaintext -d '{ \"name\": \"Johnny\"}' -import-path ".\ServerReflection.Service\Protos\" -proto Greeter.proto localhost:5001 Greet.Greeter/GetGreeting
Write-Host "--- Press any key ---"
Read-Host
