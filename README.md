# 🧪 Test Task for EKTOS

**Author:** Kolomiiets Pavlo

---

## ⚙️ Setup Instructions

1. **Rename the configuration file**  
   Change:

   ```
   appsettings-example.json
   ```

   to:

   ```
   appsettings.json
   ```

2. **Update the connection string**  
    Inside `appsettings.json`, set your MongoDB connection string in the following format:

   ```json
   {
     "MongoDb": {
       "Windows": {
         "ConnectionString": "mongodb+srv://Tester:YOUR_PASSWORD@cluster0.oxfma.mongodb.net/?authSource=admin&appName=Cluster0"
       },
       "Other": {
         "ConnectionString": "mongodb://Tester:YOUR_PASSWORD@cluster0-shard-00-00.oxfma.mongodb.net:27017,cluster0-shard-00-01.oxfma.mongodb.net:27017,cluster0-shard-00-02.oxfma.mongodb.net:27017/?ssl=true&replicaSet=atlas-idcd4-shard-0&authSource=admin&retryWrites=true&w=majority&appName=Cluster0"
       }
     },
     "DbName": "BinaryDB",
     "CollectionName": "Items"
   }
   ```

---

✅ You’re now ready to run the project.
