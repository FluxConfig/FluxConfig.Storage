# FluxConfig.Storage deployment guidance

## Getting started

### 0. Prerequisites

**Docker and docker-compose installed on the system.**

**TLS certificate in .pfx format and password for .pfx file**

### 1. Download deployment script

```bash
curl -LJO https://raw.githubusercontent.com/FluxConfig/FluxConfig.Storage/refs/heads/master/deployment/boot_storage.sh
```

### 2. Fill .cfg file as it shown in [template](https://github.com/FluxConfig/FluxConfig.Storage/blob/master/deployment/storage.template.cfg)

**You can create and fill it manually**

**Download and fill it manually**

```bash
curl -LJO https://raw.githubusercontent.com/FluxConfig/FluxConfig.Storage/refs/heads/master/deployment/storage.template.cfg
```

**Or let the script download it.**

```bash
./boot_storage.sh -c "non-existing.cfg"
Fetching docker-compose.yml...
docker-compose.yml successfully downloaded

Config file not found. Do you want to download template file? y/n
y

Fetching storage.template.cfg...
storage.template.cfg successfully downloaded

Please fill the configuration file and run this script again.
```

### 2.1 .cfg arguments

| **Argument** | **Description**                                                                                                                                                                                                                                                               | **Example**                         |
|--------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------|
| **FC_TAG**         | Tag/version of the FluxConfig.Storage image which will be used <br> You can find all tags [here](https://hub.docker.com/r/fluxconfig/fluxconfig.storage/tags)                                                                                                                 | 1.0-pre                             |
| **STORAGE_API_PORT**      | Port for api, which will be exposed from container                                                                                                                                                                                                                            | Any free port, e.g 7077             |
| **EXTERNAL_CERT_PATH**      | Path to .pfx TLS cert from build directory, <br> which will be mounted to "/https/certs/" directory within container                                                                                                                                                          | ~/dev-certs/https/cert.pfx          |
| **INTERNAL_CERT_PATH**      | Path to .pfx TLS cert from within container, <br> "/https/certs/<path to .pfx from mounted dir>"                                                                                                                                                                              | /https/certs/cert.pfx               |
| **CERT_PSWD**         | Password for .pfx TLS certificate                                                                                                                                                                                                                                             | password                            |
| **FCM_BASE_URL**      | Address of FluxConfig.Managment service                                                                                                                                                                                                                                       | http://fcmanagement:8080            |
| **FC_API_KEY**         | Internal api-key for interservice communication <br> If you don't have one, leave this argument empty and it will be generated for you. <br> Remember it for deployment the rest of the FluxConfig system <br> If you already have one from other deployments - fill argument | e72eb4601d5a45bd9d5fd8b439b9097f |
| **MONGO_USERNAME**         | Username for internal MongoDB connection, fill it or leave empy for auto generation                                                                                                                                                                                           | MongoUser                           |
| **MONGO_PASSWORD**         | Password for internal MongoDB connection, fill it or leave empy for auto generation                                                                                                                                                                                           | MongoPassword                       |

### 3. Execute deployment script

**Give executable permissions to the file**

```bash
chmod +x boot_storage.sh
```

**Deploy**

```bash
./boot_storage.sh -c "PATH TO YOUR .cfg FILE"
```

**Example of output for successful deployment**

```bash
Fetching docker-compose.yml...
docker-compose.yml successfully downloaded

Reading .cfg file...
Loaded: FC_TAG=1.0-pre
Loaded: STORAGE_API_PORT=7046
Loaded: EXTERNAL_CERT_PATH=~/.aspnet/dev-certs/https
Loaded: INTERNAL_CERT_PATH=/https/certs/aspnetapp.pfx
Loaded: CERT_PSWD=12345
Loaded: FCM_BASE_URL=http://host.docker.internal:7070

Generating value for FC_API_KEY
Loaded: FC_API_KEY=9f80196e22464b4d94e8f15b3bc509ed

Generating value for MONGO_USERNAME
Loaded: MONGO_USERNAME=u153c560bf8f84d71a68610ee99b3aa98

Generating value for MONGO_PASSWORD
Loaded: MONGO_PASSWORD=4e0dc185d80244629985d1274636bfcb

Starting FluxConfig Storage...
[+] Running 12/12
 ✔ database Pulled 
 ✔ storage_api Pulled 
[+] Running 4/4
 ✔ Network deployment_fcs-network      Created 
 ✔ Volume "deployment_fcs-mongo-data"  Created 
 ✔ Container fc-mongo                  Started 
 ✔ Container fc-storage                Started 
```

