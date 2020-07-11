# BIMSocket

*Keeping you conected*
=====================================================

<img src="kanganru.png" align="left" width="100" height="150" />

This project has been developed in October 2019 during the AEC Tech Hackathon organized by TTCore Studio with the following Sponsors that we thank you so much for the great resource they were able to provide. 
[Presentation](https://docs.google.com/presentation/d/1PY0RU9T0NnZnrGIihKm32UpJGw9H2y3DOEGgaJlzG2o/edit?ts=5dab4ee7#slide=id.g640068be55_0_5)



## Authors
* **Alberto Tono** - *San Francisco Computational Design Institute* - [SFCDI](https://www.sfcdi.org)
* **Valentine Noves** - *ENGwork* 
* **Constantina Tsiara** - *Workshop / APD* 
* **Pablo Derendinger** - *ENGwork*
* **Jeffrey Moser** - *Grimshaw*
* **Lexi Fritz** - *Tetra Tech*
* **Rachel Hartley** - *Autodesk*

Feature
* 3D Paradigm Shift Enabler
* Client Presentation
* Sketches Educational App


Presentation: 
https://docs.google.com/presentation/d/1uL1N66qP9okW5KsW7J7cxcwzE3m1eett8EwkJiCczc4/edit?usp=sharing


We developed the back end part of the Outback project that has been divided in 2 parts: 

* GANgaru
* Joey


## Getting Started

Are you a designer who sketches at client and team meetings? Or do you just not have the time or the knowledge to 3D model? The Outback family has developed two new programs that are here to help. First we have Joey, a program that will take your iPad sketch and convert it into a 2D rendering. Then there is GANgaru, a program that will take that rendering and jump into a 3D model with the simple press of a button!
The AEC industry is moving towards a 3D environment and it’s important that designers are able to keep up, but a lot of firms are not able to work in 3D and adapt due to limitations such as funding and firm size. The Outback applications will help these users by reducing the cognitive load at the beginning of the process, thus producing a faster method to reaching the 3D model. Using three essential layers, the functionality of GANgaru first starts by dissecting the user’s image by RBG and depth. Next, the image is cleaned with segmentation, contour end, edge detection, and a heat map to further emphasize the components necessary to produce the 3D model. The final function of GANgaru is the 3D model itself, which is produced using shape net and a HSP grid octress to form the final computation graphic. This model is then able to produce real-time data for the user, such as exact square footage which can then produce the cost of a proposed project.
The GANgaru application is a smart and effective solution to an industry that is heading fast into the 3D environment. At the end of the day, client satisfaction is key, and by being able to provide a client with an instant 3D model of their project idea, the Outback family is helping improve that satisfaction and helping the user stand out amongst competitors.



### Prerequisites





# References

https://arxiv.org/pdf/1512.03012.pdf
https://venturebeat.com/2018/06/14/googles-deepmind-develops-ai-that-can-render-3d-objects-from-2d-pictures/
https://runwayml.com/ 
https://storage.googleapis.com/deepmind-media/papers/Neural_Scene_Representation_and_Rendering_preprint.pdf
https://science.sciencemag.org/content/360/6394/1204.full?ijkey=kGcNflzOLiIKQ&keytype=ref&siteid=sci
https://deepmind.com/blog/article/neural-scene-representation-and-rendering
https://colmap.github.io/
https://www.youtube.com/watch?v=BjwhMDhbqAs&t=15s
https://arxiv.org/abs/1704.00710
https://shubhtuls.github.io/
https://shubhtuls.github.io/papers/pami19hsp.pdf
https://github.com/iro-cp/FCRN-DepthPrediction
https://scikit-image.org/docs/dev/user_guide/tutorial_segmentation.html
B. Curless and M. Levoy. A volumetric method for building complex models from range images. In Conference on Computer graphics and interactive techniques (SIGGRAPH), 1996.
K. Kolev, M. Klodt, T. Brox, S. Esedoglu, and D. Cremers. Continuous global optimization in multiview 3d reconstruction. In International Workshop on Energy Minimization Methods in Computer Vision and Pattern Recognition (EMMCVPR), 2007
K. N. Kutulakos and S. M. Seitz. A theory of shape by space carving. International Journal of Computer Vision (IJCV), 2000.
V. Lempitsky and Y. Boykov. Global optimization for shape fitting. In Conference on Computer Vision and Pattern Recognition (CVPR), 2007.
C. Zach, T. Pock, and H. Bischof. A globally optimal algorithm for robust tv-l 1 range image integration. In International Conference on Computer Vision (ICCV), 2007.


## License
