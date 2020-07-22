<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/LogoGithub.png' width='400' alt='BIMSocket logo' />


*Keeping you conected*
=====================================================

💬 [Join the BIMSocket community](https://aec-hackathon.slack.com/archives/C016TLPNEJH)

This project has been developed in July 2020 during the AEC Tech Hackathon.You can find the presentation of the idea
[Here](https://docs.google.com/presentation/d/1e8JpqT0Mbv7d2FD3T50pKj_9IKl_7vQu7V7U3cBJq-k/edit?usp=sharing)

*BIMSOCKet Main Goal*
=====================================================

[BIMSOCKet](https://bimsocket.rocks/) main goal is not only to communicate programs but to enhance the way we work in our industry. Our ultimate goal is create processes and workflows between all the different programs and make them work as whole.

<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/PartsRobot.jpg' width='250' alt='Conections logo' />

## What is BIMSOCKet for? ##

<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/Connections-01.png' width='350' alt='Conections logo' />

BIMSOCKet is a bidirectional cloud socket to connect any type of 3D software in the industry in real time. For the hackathon we will connect Revit, a Three JS viewer and Unity (maybe be Rhino as well) to our socket and show how it works.

## How does it work under the hood? ##

[BIMSOCKet](https://bimsocket.rocks/) allows you to create a [Firebase realtime database](https://firebase.google.com/) and then use addins on each software to connect to it in real time and save changes.

<img src='https://github.com/vnoves/BIMSocket/blob/master/Socket/Resources/FirebaseConeection-01.png' width='250' alt='Conections logo' />

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

For more details go to [Contribute](https://github.com/vnoves/BIMSocket/blob/master/CONTRIBUTING.md)

## Getting Started

Visit the wiki [Getting started page](https://github.com/vnoves/BIMSocket/wiki/Getting-started)

## Authors
* [ **Valentin Noves** - *ENGworks*](https://www.linkedin.com/in/novesvalentin/)<br/>
* [**Pablo Derendinger** - *Engworks*](https://www.linkedin.com/in/pablo-derendinger/)<br/>
* [**Alexander Corral** - *ENGworks*](https://www.linkedin.com/in/ivan-alexander-corral-aab16412b/)<br/>
* [**Jason Ekensten** - *Resorts World Las Vegas*](https://www.linkedin.com/in/jason-ekensten-787b1933/)<br/>

## License
[MIT Licence](https://github.com/vnoves/BIMSocket/blob/master/LICENSE)
