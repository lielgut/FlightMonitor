<p align="center">
<img src="images/FlightMonitor.png" alt="Logo" height="100">
</p>

![forthebadge](https://img.shields.io/badge/Made%20with-C%23-brightgreen)
![forthebadge](https://img.shields.io/badge/Made%20with-C%2B%2B-blue)

## About

This is a project for Advance Programming 2 course in Bar-Ilan directed by Eli Khalastchi. Our application can visualize a given flight data (in a CSV format) using graphs and diferent gauges and interact with the flight simulator "FlightGear".<br>
This application uses [FlightGear](https://www.flightgear.org/) Flight Simulator, which is a free, open-source program that anyone can download onto their computer (works for Mac, Windows and Linux). This program simulates a plane in various modes, such as taking off, landing and flying. It has many features such as viewing the plane from different angles, changing the time of day and even change the location!

## Key Features
- ### Graphs:
  - Our program can show the
<!-- test -->
```

```
- `hey`
---
<!-- test -->

## Dependencies
- FlightGear [Download Link](https://www.flightgear.org/download/) or download other versions [here](https://sourceforge.net/projects/flightgear/files/) [Tested and working on 2018.3.1 and above]
- Add playback_small.xml to data/protocol folder in your FlightGear installation path.
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- .NET 5??????????????????
- [MaterialDesignTheme](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) v4.0
- [MaterialMessageBox](https://github.com/denpalrius/Material-Message-Box) v4.0.2
- [OxyPlot](https://oxyplot.github.io/) v2.0
- Microsoft Expression Drawing v1.0
## Installation

Make sure you have the above installed, then open the project (using the project file "ex1.csproj"). Now compile the project using Visual Studio and wait for the application to run.<br>

## Running & Usage
You will be presented with the configuration window where you should specify the settings for the application to run with.
- Correlation threshold - The threshold which the application will use to 
- Load anomaly detection dll - This is the plugin dll the application will use to detect and show all of the anomalies from the data given.
- Load normal flight CSV - This CSV file will be used to learn the flight data and base its detections on. (The application will learn what's defined as normal flight data so it can later let us know where an anomaly from the normal data has occured)
- Load new flight CSV - This CSV file is the data the you want to check against the normal flight data for checking if there are any anomalies.
- FlightGear installation folder - This is the location where FlightGear is installed on your drive. It'll be used for opening FlightGear it isn't running already. (You'll need to specify a port which will be used for opening the FlightGear server and will set the application's port to as a client)<br>
<b>FlightGear must be opened either using the button in the application or using the FlightGear app and adding the settings provided.</b> These settings are used to open the FlightGear server, specifying for FlightGear the XML to use to parse the data being sent to it, and setting the time of day to morning just for easier viewing purposes.

## UML Chart
![UML Chart](images/uml.svg)