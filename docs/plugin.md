
## Creating an anomaly detection plugin
The plugins folder in the project includes two different plugins:
- <b>LinearReg.dll</b> - uses linear regression line of two correlated features to detect the anomalies, returns an annotation of the line.
- <b>MinCircle.dll</b> - uses the minimal circle of two correlated features to detect the anomalies, returns an annotation of the circle.

You can create your own plugin by using any anomaly detection alogrithm you'd like. To do so, follow these instructions:
1. create a new C# class library project in Visual Studio.
2. Install OxyPlot.Wpf 2.0.0 from the Nuget Package Manager.
2. Create a new class, with both the namespace and class name being "Detector".
3. Implement the following methods:

```csharp
// the class constructor
public Detector();

// used for releasing any memory that was allocated in the program
public void deleteAnomalyHelper();

/* should load the CSV files, learn the correlative features from the first file,
and use the second file to detect its anomalies. 
The threshold can be used to determine the pearson threshold by which features are considered correlated.*/
public void loadFlightData(String normalFlight, String newFlight, float threshold);

// should return the name of the feature correlative to given feature
public string getCorrFeature(string featureName);

// should return if given feature was anomalous at given timestep
public bool isFeatureAnomalous(int timestep, string featureName);

/* should return an annotation, which represents the anomaly detection algorithm used.
Can be any OxyPlot.Wpf class which inherits the abstract Annotation class, 
such as FunctionAnnotation, EllipseAnnotation, PolygonAnnotation, etc.*/
public OxyPlot.Wpf.Annotation getAnnotation(string featureName);

```

5. Build the project and load the new dll located at "bin\debug\netcoreapp" into the app. Now you can use the algorithm to detect and view the anomalies.