//Load customized Json file

function loadModel() {
    var loader = new THREE.ObjectLoader();

    // Get model from firebase for the first time
    db.collection('models').doc(modelName).get().then(function(doc) {
        if (doc.exists) {
            model = doc.data();
            loader.parse(model,
                function (mod) {
                    // console.log(mod);
                    modelObj = mod;
                    console.log(modelObj);
                    mod.castShadow = true;
        
                    //Adjust the camera location to the max bouding from object
                    var bbox = new THREE.Box3().setFromObject(mod);
                    var maxObjectDistance = Math.max(bbox.max.z, bbox.max.x, bbox.max.y);
                    camera.position.z = maxObjectDistance
                    camera.position.x = bbox.max.x;
                    camera.position.y = bbox.max.y;
            
                    //Adjust the camera near plane and max/min zoom for controls
                    var averageSizes = ((bbox.max.z + bbox.max.x +  bbox.max.y)/3)*0.01;
                    camera.near = averageSizes;
                    controls.minDistance = averageSizes*1.2;
                    controls.maxDistance = maxObjectDistance*4;
                    camera.updateProjectionMatrix();
                    
                    scene.add(mod);         
                }
            );
        } else {
            console.log("No such document!");
        }
    }).catch(function(error) {
        console.log("Error getting document:", error);
    });

    // Subscribe on update changes for model on firebase
    db.collection('models').doc(modelName).onSnapshot((modelChanges) => {
        model = modelChanges.data();
        loader.parse(model,
            function (mod) {
                mod.castShadow = true;      
            }
        ); 
    });
}

