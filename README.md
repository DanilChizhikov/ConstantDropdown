# Constant Dropdown
![](https://img.shields.io/badge/unity-2022.3+-000.svg)

## Description
Constant Dropdown is a Unity editor extension that provides a customizable dropdown inspector for constant values. It allows you to create type-safe dropdowns in the Unity Inspector that are populated from constant values defined in your code, making it easier to manage and select predefined values.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
    - [Install via UPM (using Git URL)](#install-via-upm-using-git-url)
    - [Install manually (using .unitypackage)](#install-manually-using-unitypackage)
- [Usage](#usage)
    - [Basic Usage](#basic-usage)
    - [Supported Types](#supported-types)
    - [ConstantDropdown](#constantdropdown)
- [License](#license)

## Features
- Create dropdowns for string, int, and float constants
- Type-safe constant selection in the Unity Inspector
- Easy integration with existing code
- Support for custom constant classes
- Simple and intuitive API

## Installation

### Install via UPM (using Git URL)
1. Navigate to your project's Packages folder and open the manifest.json file.
2. Add this line below the "dependencies": { line
    - ```json title="Packages/manifest.json"
      "com.danilchizhikov.constatntdropdown": "https://github.com/DanilChizhikov/ConstantDropdown.git?path=Assets/ConstantDropdown#1.1.0",
      ```
UPM should now install the package.

### Install manually (using .unitypackage)
1. Download the latest .unitypackage from the [releases](https://github.com/DanilChizhikov/ConstantDropdown/releases) page
2. Double-click the downloaded .unitypackage file to import it into your Unity project

## Usage

### Basic Usage

1. Create a static class with your constants:
   - Usage with class
     ```csharp
     [ConstantSource(typeof(StringConstantClass))]
     public static class StringConstantClass
     {
         public const string Option1 = "First Option";
         public const string Option2 = "Second Option";
         public const string Option3 = "Third Option";
     }
     ```
   - Usage with collection
      ```csharp
      public static class StringConstantClass
      {
        private const string Item1 = "Item_1";
        private const string Item2 = "Item_2";
        private const string Item3 = "Item_3/Item_1/Item_1";
        private const string Item4 = "Item_3/Item_1/Item_2";
		
        [ConstantSource(typeof(StringConstantClass))]
        public static readonly string[] Items = new string[]
        {
            Item1,
            Item2,
            Item3,
            Item4,
        };
      }
      ```

2. Use the `[ConstantDropdown]` attribute in your MonoBehaviour:
    - MonoBehaviour
   ```csharp
   using UnityEngine;
   using DTech.ConstantDropdown;

   public class Example : MonoBehaviour
   {
       [SerializeField, ConstantDropdown(typeof(StringConstantClass))]
       private string myStringConstant;
   }
   ```
   - ScriptableObject
   ```csharp
   using UnityEngine;
   using DTech.ConstantDropdown;

   public class Example : ScriptableObject
   {
       [SerializeField, ConstantDropdown(typeof(StringConstantClass))]
       private string myStringConstant;
   }
   ```

### Supported Types

Constant Dropdown supports the following types:
- `string`
- `int`
- `float`

Example with different types:
```csharp
public static class IntConstantClass
{
    public const int Value1 = 1;
    public const int Value2 = 2;
    public const int Value3 = 3;
}

public static class FloatConstantClass
{
    public const float Small = 0.1f;
    public const float Medium = 0.5f;
    public const float Large = 1.0f;
}

[SerializeField, ConstantDropdown(typeof(IntConstantClass))]
private int myIntConstant;

[SerializeField, ConstantDropdown(typeof(FloatConstantClass))]
private float myFloatConstant;
```

### ConstantDropdown
- Base Class
```csharp
public abstract class ConstantDropdownBaseAttribute : PropertyAttribute
{
    public abstract Type LinkingType { get; }
    public abstract string PrefixName { get; }
}
```

| Property | Type | Description                                |
|----------|------|--------------------------------------------|
| LinkingType | Type | The type of the constant class          |
| PrefixName | string | The prefix name of the display value   |

- Default Class
```csharp
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class ConstantDropdownAttribute : ConstantDropdownBaseAttribute
{
    public override Type LinkingType { get; }
    public override string PrefixName { get; }

    public ConstantDropdownAttribute(Type linkingType, string prefixName = "")
    {
        LinkingType = linkingType;
        PrefixName = prefixName;
    }
}
```


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.