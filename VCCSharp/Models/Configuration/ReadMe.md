# Configuration
The configuration is stored as a JSON file ```(*.json)```.

The goal here is to make it as user-friendly as possible.  Most of the time data will be maintained by the application, but still laid out in a way that can be edited manually.

Data is stored in a hierarchical tree structure, starting with the ```Root``` class, and branching out.

Most data is stored as a ```string```, ```integer```, or ```boolean``` flag and are handled as such.

## RangeSelect\<T>
Some data are an enumerable type ```(enum)``` which include a range of valid values, of which one is selected.  ```RangeSelect<T>``` is used for this purpose.

Data is then stored in this format:
```
{
    "Options": "{(enum range)},
    "Selected": "(enum)"
}
```

Where ```Selected``` is the data is converted to/from a string to make it easier to read.  
And ```(Options)```, a read-only field, is added showing valid values.

## RangeSelect
Some integer values have a small range, as in which switch is selected on the Multi-PAK.  ```RangeSelect``` is used for this purpose.
Same as ```RangeSelect<T>```, but for integers.

## Persistence
This class handles the JSON serialization of the configuration as well as the Load/Save functionality.

## MultiSlots
The Multi-PAK and the Floppy Disk options both allow up to 4 simultaneious attachments through their "slots".  This class wraps a string array (0-3) into 4 distinct fields: 

```
"Slots": {
    "1": "",
    "2": "",
    "3": "",
    "4": ""
}
```
