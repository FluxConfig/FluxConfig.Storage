configdb = db.getSiblingDB("configurations")

configdb.createCollection("vault")
configdb.createCollection("realtime")

configdb.vault.createIndex(
    {
        "api_key": 1,
        "config_tag": 1
    },
    {
        unique: true
    }
)

configdb.realtime.createIndex(
    {
        "api_key": 1,
        "config_tag": 1
    },
    {
        unique: true
    }
)