# Acortador de URL. Proyecto Atlas

[Informacion Adicional - 80bits.blog](http://80bits.blog/index.php/2020/04/16/atlas-proyecto-acortador-de-url/)

## Configuraciones

En el archivo appsettings.json

````
"MongoConnectionString": "mongodb://admin:1234567890@atlas.database/atlas",
"MongoDatabaseName": "atlas",
"MongoCollection": "collection1",
"SizeCode": "4"
`````
Crear el siguiente index en la coleccion mongodb:

````
db.collection1.createIndex( { "obj": 1, "short_code": 1 }, { "v": 2, "unique": true, "name": "index_obj_short_code", "ns": "atlas.collection1", "background": true, "sparse": true });
````

## Ejecutar servicio

Se necesita instalar el runtime de NetCore 3.1, puedes descargarlo [aqui](https://dotnet.microsoft.com/download)

`````
$ dotnet restore
¢ dotnet run 
`````