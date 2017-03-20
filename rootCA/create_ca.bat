@setlocal

openssl genrsa -out my-update-center.key 1024 || exit /b 1
openssl req -new -x509 -days 1095 -key my-update-center.key -out my-update-center.crt || exit /b 1
