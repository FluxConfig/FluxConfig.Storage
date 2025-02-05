configdb = db.getSiblingDB("configurations")

configdb.createCollection("vault")
configdb.createCollection("realtime")

configdb.vault.createIndex(
    {
        "key": 1,
        "tag": 1
    },
    {
        unique: true
    }
)

configdb.realtime.createIndex(
    {
        "key": 1,
        "tag": 1
    },
    {
        unique: true
    }
)