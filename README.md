# VismaWinterTask
## Dalius Lukša

# Instaliation guide

Simply download VismaWinterTask v2.0 Release folder and open VismaWinterTask.exe.


### P.S.

The task said:

> The UI should be a Console application. Focus on good code design and good practices, UI
design should be minimalistic and simple.

This part got me confused. At first I though that the requirement was to make a Console application
 which doesn't have any UI (unless we take into account the lines with numbers near it and you navigate
 by inputing line numbers). But then the next sentence says that the UI design should be minimalistic and simple
 which means that there is suppose to be some sort of UI (with buttons and etc.). So in the end I decided to make a 
 Windows Form app in order to have the option to create simple UI elements.


> We require OOP design and unit tests will be considered a big advantage.

I tried to write as many Unit tests as I could, but because of the Form app I could only fully test one of the class
(CSVReader.cs) as it didn't derived from Form class. So Stock.cs, Menu.cs and Orders.cs was untested 
(I'm very inexperienced with Unit tests so I couldn't find a 'good' way to test private methods without remaking them
to public just for the sake of testing).