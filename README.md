<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/LogoGithub.png' width='400' alt='BIMSocket logo' />


*Keeping you conected*
=====================================================

💬 [Join the BIMSocket community](https://aec-hackathon.slack.com/archives/C016TLPNEJH)

This project has been developed in July 2020 during the AEC Tech Hackathon.You can find the presentation of the idea
[Here](https://docs.google.com/presentation/d/1e8JpqT0Mbv7d2FD3T50pKj_9IKl_7vQu7V7U3cBJq-k/edit?usp=sharing)


## What is BIMSOCKet for? ##

<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/Connections-01.png' width='350' alt='Conections logo' />

BIMSOCKet is a bidirectional cloud socket to connect any type of 3D software in the industry in real time. For the hackathon we will connect Revit, a Three JS viewer and Unity (maybe be Rhino as well) to our socket and show how it works.

## Features and design principles ##

- **Automatic synchronization**.  We're not yet making an effort to support old platforms, but we have tested BIMsocket in Node.js, Chrome, Firefox, Safari, MS Edge, and Electron

- **Database Storage**. When you use BIMSOCKet everything is saved in a database, you any change you make while you are offline will be updated as soon as you connect again.

- **Format Agnostic**. BIMSOCKet currently use Json as the exchange format but any other format like IFC, glTF, etc. can be use as well.

- **Multi Model Capability**.You can create multiple Sockets and then upload all of them together so every modification will only affect the model origin.

- **Useful API**. ith the BIMSOCKet API you can not only share information but also process it or modified before it reaches any program.

- **Easy plugin**. Create a project in BIMSOCKet is easy, just create a database, install your software node and then select the created database.




## Contribute ##

BIMSocket is an open-source project. You can make suggestions or track and submit bugs via Github issues.  You can submit your own code to the BIMSocket project via a Github pull request.

Clone this repo

``` git clone https://github.com/vnoves/BIMSocket.git```

Load submodules

``` git pull --recurse-submodules ```

Refresh submodules
``` git submodule update --init --recursive ```

``` git submodule update --recursive --remote ```

## Getting Started

Visit the wiki [Getting started page](https://github.com/vnoves/BIMSocket/wiki/Getting-started)

## Versions
Json:
[V3](https://github.com/json-schema-org/json-schema-spec)

Unity:
[2019.3.0a7 (64-bit)](https://store.unity.com/#plans-individual)<br/>

Revit:
[2020 20.0.0.377 20190327_2315(x64)](https://www.autodesk.com/education/free-software/revit)<br/>

Three.js:
[103](https://github.com/mrdoob/three.js/releases/tag/r103)<br/>

Rhino:
[6](https://www.rhino3d.com/download/rhino-for-windows/6/latest)<br/>


## References
Json:
https://github.com/va3c/RvtVa3c

Unity:
https://firebase.google.com/docs/unity/setup

Revit:
https://github.com/jeremytammik/DirectObjLoader

Three.js:
https://threejs.org/
https://threejs.org/docs/#api/en/loaders/ObjectLoader

Unity:
https://github.com/SaladLab/Json.Net.Unity3D
https://github.com/Mcklem/JSONGameObject

## Authors
* [ **Valentin Noves** - *ENGworks*](https://www.linkedin.com/in/novesvalentin/)<br/>
* [**Pablo Derendinger** - *Engowrks*](https://www.linkedin.com/in/pablo-derendinger/)<br/>
* [**Alexander Corral** - *ENGworks*](https://www.linkedin.com/in/ivan-alexander-corral-aab16412b/)<br/>
* [**Jason Ekensten** - *Resorts World Las Vegas*](https://www.linkedin.com/in/jason-ekensten-787b1933/)<br/>

## License
[MIT Licence](https://github.com/vnoves/BIMSocket/blob/master/LICENSE)
