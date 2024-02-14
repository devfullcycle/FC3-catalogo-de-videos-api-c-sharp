#!/bin/bash
docker network create --driver bridge catalog_api || true
docker network create --driver bridge service_catalog || true

docker-compose -f ./elk/docker-compose.yml up -d 
docker-compose -f ./kafka/docker-compose.yml up -d 