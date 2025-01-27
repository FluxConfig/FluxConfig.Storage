FROM mongo:8.0

ENV MONGO_INITDB_DATABASE="configurations"
COPY ["./mongo-init.js", "/docker-entrypoint-initdb.d/"]

RUN chmod +x /docker-entrypoint-initdb.d/mongo-init.js