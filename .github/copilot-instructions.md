# GitHub Copilot Instructions for Animal Genetics (Continued) Mod

## Mod Overview and Purpose

Animal Genetics (Continued) is a mod for RimWorld that enhances the genetic traits of animals. By introducing seven inheritable genetic traits, the mod aims to allow players to engage in selective breeding, resulting in animals that are stronger, faster, and more productive in terms of resource yields like meat, leather, milk, and wool. The goal is to simulate a dynamic ecosystem within the game where players can cultivate superior animal breeds to meet their needs.

## Key Features and Systems

- **Genetic Traits**: Introduces seven new inheritable genetic traits: Health, Damage, Speed, Capacity, Meat, Leather, and Milk/Wool. Each trait can be adjusted via mutation factors.
- **Selective Breeding**: Animals can pass down their genetic traits to their offspring. Players can breed animals to achieve desired traits.
- **Gene Display**: Adds new Genetics tabs that display detailed information about each animal's genetic makeup.
- **Mutation Control**: Players can adjust gene ranges and mutation factors within the mod options.
- **Compatibility with Humans**: Includes optional support for applying the same genetic traits to humanlike pawns, compatible with mods that enable human reproduction.
- **Visual Representation**: Offers two different color schemes to represent gene values for better user experience.

## Coding Patterns and Conventions

- **Class Design**: Follows a component-based architecture, making use of static and non-static classes for different functionalities.
- **Modularity**: Features are divided into various components for better readability and maintainability. 
- **Naming Conventions**: Classes and methods are named using PascalCase. Private methods and variables use camelCase.
- **Inheritance**: Utilizes inheritance for extending base functionality, such as genetic information classes that derive from ThingComp.

## XML Integration

- **Data-Driven**: The mod uses XML for defining traits and compatibility features with other mods. XML files define the characteristics, gene mutations, and options available within the game.

## Harmony Patching

- **Patch Application**: The mod uses Harmony, a library for patching compiled code, to modify and extend the functionality of RimWorld without changing the original source files.
- **Systematic Patching**: Harmony patches are applied systematically to ensure compatibility with other mods. The mod includes patches for egg laying speeds and resource amounts.
- **Compatibility**: Extensive testing has been done with popular animal-related mods to ensure smooth integration and compatibility.

## Suggestions for Copilot

- **Code Completion**: Use Copilot to assist in filling in method bodies and class implementations, especially for adding new features or refactoring existing code for better efficiency.
- **Patching Operations**: Utilize Copilot for generating Harmony patching code by suggesting possible patches or optimizations on the current RimWorld methods.
- **UI Development**: Copilot can help suggest patterns for UI development, particularly when creating or updating the Genetics tab and other interfaces.
- **XML Handling**: Leverage Copilot to generate XML tags and structures required for defining new genetic traits and options.
- **Debugging and Logging**: Enhance error handling and logging by letting Copilot propose try-catch patterns or debugging information.

This document serves as a guideline for working with the Animal Genetics (Continued) mod and illustrates how GitHub Copilot can be leveraged to enhance the development workflow effectively.
