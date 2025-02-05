#!/bin/bash

help_description()
{
  echo "Example of usage :"
  echo "./boot_storage.sh -c <path_to_config_file>"
  echo ""
  echo "Arguments description :"
  echo "<path_to_config_file> - path to FluxConfig storage .cfg file"
  echo "To learn about it's structure visit https://github.com/FluxConfig/FluxConfig.Storage/blob/dev/version-1.0/deployment/DEPLOY.md"
}

boot_script()
{
  # Parse CLI Arguments
  ########
  while getopts "c:h" opt
    do
      case "$opt" in
        c ) local pathToConfig="$OPTARG" ;;
        h ) 
          help_description
          exit 0  ;;
        ? ) 
          help_description
          exit 1  ;;
      esac
  done
    
  if [ -z "$pathToConfig" ]; then 
    echo "Missing -c CLI argument."
    help_description
    exit 1
  fi
  ########
  
  # Check docker installation
  ########
  if ! command -v docker 2>&1 >/dev/null
    then
        echo "Docker could not be found. Install Docker first."
        exit 1
  fi
    
  if ! command -v docker-compose 2>&1 >/dev/null
    then
        echo "Docker-compose could not be found. Install Docker-compose first."
        exit 1
  fi
  ########
  
  # Fetching compose file
  ########
  local COMPOSE_URL="https://raw.githubusercontent.com/FluxConfig/FluxConfig.Storage/refs/heads/dev/version-1.0/deployment/docker-compose.yml"
  echo "Fetching docker-compose.yml..."
  curl -LJO $COMPOSE_URL || { echo "Failed to fetch docker-compose.yml"; exit 1; }
  echo "docker-compose.yml successfully downloaded"
  echo ""
  ########
  
  
  # Check config file existence and download if needed
  ########
  if [ ! -f "$pathToConfig" ]
  then
    local downloadAc
    echo "Config file not found. Do you want to download template file? y/n"
    read downloadAc
    if [ "$downloadAc" == "y" ]; then
      local TEMPLATE_URL="https://raw.githubusercontent.com/FluxConfig/FluxConfig.Storage/refs/heads/dev/version-1.0/deployment/storage.template.cfg"
      echo ""
      echo "Fetching storage.template.cfg..."
      curl -LJO $TEMPLATE_URL || { echo "Failed to fetch docker-compose.yml"; exit 1; }
      echo "storage.template.cfg successfully downloaded"
      echo "Please fill in the configuration file and run this script again."
      echo ""
      exit 0
    fi
  fi
  ########
  
  # Load cfg variables to .env
  ########
  if [ ! -f ".env" ]
  then
    touch ".env"
  fi
  truncate -s 0 ".env"
  
  echo "Loading .cfg file..."
  while IFS= read -r line || [[ -n "$line" ]]; do
      if [[ "$line" =~ ^#.*$ || -z "$line" ]]; then
          continue
      fi
  
      key=$(echo "$line" | cut -d '=' -f 1)
      value=$(echo "$line" | cut -d '=' -f 2-)
  
      key=$(echo "$key" | xargs)
      value=$(echo "$value" | xargs)
      
      if [ "$key" == "MONGO_USERNAME" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for MONGO_USERNAME"
        value=$(uuidgen)
      fi

      if [ "$key" == "MONGO_PASSWORD" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for MONGO_PASSWORD"
        value=$(uuidgen)
      fi

      if [ "$key" == "FC_API_KEY" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for FC_API_KEY"
        value=$(uuidgen)
      fi
    
      printf "$key=$value\n" >> .env
      echo "Loaded: $key=$value"
  done < "$pathToConfig"
  ########
  
  # Booting the application 
  ########
  echo ""
  echo "Starting the application..."
#  docker-compose up -d
  exit 0
  ########
}


boot_script "$@"