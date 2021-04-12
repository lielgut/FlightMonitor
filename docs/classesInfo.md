
## Additional Documentation

The project implements the MVVM design pattern, being seperated to 3 major parts - The views, which are responsible for presenting the visual part of the app, the ViewModels, which are responsible for the presentation logic required for the view, and the model which consists of all of the classes that are responsible for the program's logic and data.

#### The Views
- <b>ConfigWindow</b> - a window showing up at startup of the application. It allows the user to choose a correlation threshold, load the dll plugin and CSV files, determine the FlightGear installation folder and launch it. It contains a SettingsViewModel in order to load the required information to the program's model after pressing the "done" button, which will create and show and MainWindow. Error messages could show in case of invalid input or failure to connect to the server.
- <b>MainWindow</b> - the main window of the program, which contains the user-controls responsible for presenting the different views. The MainWindow class creates the ViewModels neccesary for each view at its initialization. It allows changing between views by using the tab control.
- <b>DataView</b> - visually presents the following information: altimeter, air-speed, heading-degree, yaw-degree, roll-degree, pitch-degree, throttle, rudder, aileron and elevator. Contains an instance of the DataViewModel.
- <b>ResearchView</b> - includes a listbox for selecting one of the 42 features. After selecting a feature, the upper-left graph presents the value of the selected feature, updated constantly over time. The upper-right graph shows the same graph for its correlative feature, if such exists.
the bottom graph shows the correlation graph, which includes data points from recent 30 seconds of the flight, with the X-Axis representing the value of the selected feature, and the Y-Axis representing the value of its correlative feature. Additional annotation will be shown according to the loaded anomaly detection algorithm (such as a line or a circle) and the anomalous points are colored in red. The user can use the right mouse button to drag the position of the graph, use the mouse wheel over the axes to change their proportion, zoom in/out with the zoom buttons or the mouse wheel, or click the refresh button to reset the graph view and update the shown annotation after loading a different plugin. The listbox on the right shows a list of timesteps where the anomalies were the detected, which can be pressed to go back and view the anomalies.
This class contains an instance of the ResearchViewModel.
- <b>PlayerView</b> - includes a slider for changing current timestep of the flight, a slider for changing the speed of the flight, a play/pause button for resuming or pausing the flight, fast-forward/rewind buttons, stop button, etc. Contains an instance of the PlayerViewModel.
- <b>SettingsView</b> - allows changing settings of the program, including loading new CSV files, loading a new plugin, setting a different threshold for anomaly detection, setting a different port and launching FlightGear. Contains an instance of the SettingsViewModel.

#### The ViewModels
- <b>ViewModel</b> - a general ViewModel class, inherited by all other ViewModels and implements the INotifyPropertyChanged interface. Contains an instance of the model (IFlightControl).
- <b>DataViewModel, ResearchViewModel, PlayerViewModel</b> and <b>SettingsViewModel</b> - include all the properties used for getting the data that is presented in the matching view, by using the model which is a class member. Data-binding is used to get the data from the ViewModels which is shown in the views.

#### The Model

- The <b>IFlightControl</b> interface, implemented by <b>FlightControl</b>, is the facade of the program's model. It is contained within the different ViewModels and is responsible for the entire program logic by interacting with other parts of the model, which are listed below.
- The <b>IFlightData</b> interface, implemented by <b>FlightData</b>, is responsible for saving and returning the flight data which is presented in the DataView.
- The <b>IClient</b> interface, implemented by <b>SimpleClient</b> is responsible for general client-server communication via TCP connection, in order to transfer the required data.
- The <b>Pilot</b> abstract class, inherited by the <b>SimplePilot</b> class, is responsible for saving the necessary lines from the CSV files and send them to the FlightGear server via the Client class which is contained as a member, in order to make the plane fly (hence its name). It is seperated from the Client class in order to seperate their responsibilities and keep the client more general.
- The <b>IResearch</b> interface, implemented by the <b>Research</b> class, is responsible for mapping between each feature and its necessary information saved in the inner <b>ResearchInfo</b> class (such as DataPoints used for the graphic represenation, correlative feature name, anomalies and the matching annotation), and for dynamically loading a dll plugin that is responsible for anomaly detection.
- The <b>PathInfo</b> class is responsible for saving the paths of loaded files.

#### The Data Flow
1. The configuration window creates the SettingsViewModel which contains an instance of FlightControl, used with the interface IFlightControl. After pressing done the data is loaded into the model via the SettingsViewModel which uses this interface.
Each line of the CSV file (saved as strings) containing the data of the new flight is stored in the Pilot class, the parsed values (saved as floating-point numbers) are saved in the FlightData class (which is interacted with by using the IFlightData interface). These values are also saved as DataPoints in the Research class (implements IResearch) in order to be used by the ResearchViewModel to later present the graphs.
2. The anomaly detection algorithm is loaded from the dll in order to analyze the data and detect the anomalies, which are saved in the Research class, by using a mapping between each feature and its data.
3. After pressing the play button in the player view, the FlightControl class starts a new thread in which the connection with the server is established, and data is constantly being sent while the timestep of the flight is updated.
4. Updating the Timestep property, present in the FlightControl class, notifies the different properties in the ViewModels which require changing their state when the timestep changes. This way they get the required data from the model, and the views are updated as FlightGear simulates the course of the flight.
5. The flight is paused when reaching the last timestep or when the pause button is clicked. The user can play the flight again, go back to any time and view the data.
6. The user can use the settings to load a new flight or try a different plugin, which will load/analyze the data again with the process described above.

## UML Chart
![UML Chart](../images/uml.svg)