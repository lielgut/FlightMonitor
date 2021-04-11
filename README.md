<p align="center">
<img src="images/FlightMonitor.png" alt="Logo" height="100">
</p>

![forthebadge](https://img.shields.io/badge/Made%20with-C%23-brightgreen)
![forthebadge](https://img.shields.io/badge/Made%20with-C%2B%2B-blue)

## About

This is our project for "Advanced Programming 2" course in Bar-Ilan University. Our application can load CSV files representing flight data, visualize the plane's state and controls, connect to "FlightGear" flight simulator for simulating the course of the flight, analyze it to detect anomalies with a dynamically loaded anomaly detection algorithm and present the results with different graphs.<br>
[FlightGear](https://www.flightgear.org/) Flight Simulator is a free, open-source program for Mac, Windows or Linux. It simulates a plane in various modes, such as taking off, flight and landing. It includes features such as viewing the plane from different angles, changing the time of the day or the location.

## Key Features
- ### Connecting to FlightGear:
  - Our program can connect to FlightGear and send the flight data to simulate the course of the flight. The user can use the player bar in order to change the current timestep of the flight, and use the different controls to play, pause, fast forward or rewind.
- ### Presenting Data:
  - The data tab in our program visually presents various data during the flight, such as altimeter, air speed, roll, pitch and yaw degrees, heading degree, the aileron, elevator, throttle and rudder controls.
- ### Visualizing Anomalies:
  - The research tab allows flight researches to analyze anomalies in a flight, by using another flight data for learning the correlation between different features, and using an anomaly detection algorithm which can be dynamically loadad via dll. The user can select a feature to present its values in a constantly updating graph, alongside its correlated feature if one exists. The correlation between the features is presented dynamically according to loaded algorithm (for example by showing a linear regression line or minimal circle). The values of the recent 30 seconds are presented with the anomalies highlighted in red. The user can see the list of timesteps during flight with anomalous values and choose to go back to these anytime.
  
<!-- test -->
```

```
- `hey`
---
<!-- test -->

## Dependencies
- Either the latest [FlightGear Simulator](https://www.flightgear.org/download/) or any older version from 2018.3.1 and above.
- The playback_small.xml file which can be found in the "Resources" folder is required to be located at the data/protocol folder in FlightGear installation folder.
- Latest version of [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- .NET 5.0
</br>
#### Libraries used:
- [MaterialDesignTheme](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) v4.0
- [MaterialMessageBox](https://github.com/denpalrius/Material-Message-Box) v4.0.2
- [OxyPlot](https://oxyplot.github.io/) v2.0
- Microsoft Expression Drawing v1.0

## Installation

Make sure you have all of the above installed.
Clone the project using:
```
git clone https://github.com/lielgut/ex1.git
```
then open the project via Visual Studio 2019, build and run the application.<br>

## Running & Usage
When running the application you will be presented with the configuration window where you should enter the following:
- Correlation threshold - a threshold by which the anomaly detection algorithm will determine which features are correlated.
- Anomaly detection DLL path - the plugin that the application will use to learn correlations, detect and present the anomalies.
- Normal flight CSV path - a CSV file with data of a normal flight is required for the algorithm to learn which values are considered noraml, in order to later recognize anomalies.
- New flight CSV path - a CSV file with data of the new flight you'd like to show on FlightGear, present its data and analyze it for anomalies detection.</br>
- FlightGear installation folder - The location where FlightGear is installed on your computer. Will be used to make sure the required playback_small.xml file is found in the proper location. You can also choose to launch FlightGear via the configuration window. (You have to specify a port which will be used for opening a FlightGear server). Alternatively you could launch FlightGear manually, and copy the settings presented in the configuration window to your FlightGear program before pressing "fly".<br>
<b>FlightGear must be running at chosen port in order for the program to run.</b> These settings are used to open the FlightGear server, specifying the XML file for FlightGear to use 
in order to map the values to its matching features. For easier viewing of the flight the time of the day setting is set to morning, but can be changed via FlightGear.

## UML Chart
![UML Chart](images/uml.svg)