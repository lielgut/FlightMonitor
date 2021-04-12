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

## Project Files
The project files include the following:
- The <b>Views</b> folder includes the multiple views in the project: DataView, PlayerView, ResearchView, SettingsView (UserControls) and also the configuration window (which is shown at startup). Each one includes both XAML and CS files.
- The <b>ViewModels</b> folder includes all the ViewModel classes for the views above: DataViewModel, PlayerViewModel, ResearchViewModel and SettingsViewModel. All of the view models extend the general ViewModel class, and are responsible for presentation logic.
- The <b>Model</b> folder includes all of the classes and interfaces which are part of the program's model:
  - The <b>IFlightControl</b> interface, implemented by the <b>FlightControl</b> class.
  - The <b>IFlightData</b> interface, implemented by the <b>FlightData</b> class.
  - The <b>IClient</b> interface, implemented by the <b>SimpleClient</b> class.
  - The <b>Pilot</b> abstract class, inherited by the <b>SimplePilot</b> class.
  - The <b>IResearch</b> interface, implemented by the <b>Research</b> class.
  - The <b>PathInfo</b> class.
- The <b>MainWindow</b> class and XAML.
- The <b>Plugins</b> folder contains 2 DLL files, which are anomaly detection plugins that can be loaded into the program. Both were written in C# and use a dll written in C++. Each one is responsible for using a different anomaly detection algorithm and create its graphics accordingly. Additional info on plugins for the app can be found [here](docs/plugin.md).
- The <b>Resources</b> folder includes additional resources required for the project (such as the feature names xml and neccesary dll written in C++).
- The <b>images</b> folder contains images used in the project.
#### Additional details on each class/interface can be found [here](docs/classesInfo.md).

## Dependencies
- Either the latest [FlightGear Simulator](https://www.flightgear.org/download/) or any older version from 2018.3.1 and above.
- The playback_small.xml file which can be found in the "Resources" folder is required to be located at the data/protocol folder in FlightGear installation folder.
- Latest version of [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- .NET 5.0

#### Libraries used:
- [MaterialDesignTheme](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) v4.0
- [MaterialMessageBox](https://github.com/denpalrius/Material-Message-Box) v4.0.2
- [OxyPlot.Wpf](https://oxyplot.github.io/) v2.0.0
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
- <b>Correlation threshold</b> - a threshold by which the anomaly detection algorithm will determine which features are correlated.
- <b>Anomaly detection DLL path</b> - the plugin that the application will use to learn correlations, detect and present the anomalies.
- <b>Normal flight CSV path</b> - a CSV file with data of a normal flight is required for the algorithm to learn which values are considered noraml, in order to later recognize anomalies.
- <b>New flight CSV path</b> - a CSV file with data of the new flight you'd like to show on FlightGear, present its data and analyze it for anomalies detection.</br>
<b>note:</b> both CSV files must be in a CSV format, without a line with the feature names, with values for each of the 42 features seperated by commas.

- <b>FlightGear installation folder</b> - The location where FlightGear is installed on your computer. Will be used to make sure the required playback_small.xml file is found in the proper location. You can also choose to launch FlightGear via the configuration window. (You have to specify a port which will be used for opening a FlightGear server). Alternatively you could launch FlightGear manually, and copy the settings presented in the configuration window to your FlightGear program before pressing "fly".<br>
<b>FlightGear must be running at chosen port in order for the program to run.</b> These settings are used to open the FlightGear server, specifying the XML file for FlightGear to use 
in order to map the values to its matching features. For easier viewing of the flight the time of the day setting is set to morning, but can be changed via FlightGear.

## Additional information
- [More documentation and project UML](docs/classesInfo.md)
- [How to implement a plugin for FlightMonitor](docs/plugin.md)
- [User stories video](link)
