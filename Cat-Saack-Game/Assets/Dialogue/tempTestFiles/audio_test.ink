INCLUDE globals_test.ink
~ test_variable = "Started the portrait dialogue"
This is a test of the name / portrait system using tags. #character:repairman #portrait:neutral #layout:left
How's the weather looking?
* [Hot]
    Ughhh, not again. Texas summers are awful. 
    For real. Make sure to bring water with you. #character:alien #portrait:neutral #layout:right
* [Cold]
    Ooh, nice and chilly. I hope there's wind. #character:repairman #layout:left
    Then I can finally wear some layers! #character:alien #portrait:neutral #layout:right
* [Wet]
    Hey, at least there'll be rain, right? #character:alien #portrait:neutral #layout:right
    Yeah, but then it'll be muggy tomorrow. #character:repairman #portrait:neutral #layout:left

-
Whatever it'll be like tomorrow, I'll probably end up staying inside. #character:alien #portrait:neutral #layout:right
What about you? #portrait:neutral
I'll do that too, but after I can walk home. #character:repairman #portrait:neutral #layout:left #speaker:Gary
~ test_variable = "got through the dialogue"
->END