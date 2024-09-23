INCLUDE globals.ink
~ test_variable = "Started the portrait dialogue"
This is a test of the name / portrait system using tags. #character:ffion #portrait:neutral #layout:left
How's the weather looking?
* [Hot]
    Ughhh, not again. Texas summers are awful. #portrait:annoyed
    For real. Make sure to bring water with you. #character:lila #portrait:neutral #layout:right
* [Cold]
    Ooh, nice and chilly. I hope there's wind. #character:ffion #portrait:happy #layout:left
    Then I can finally wear some layers! #character:lila #portrait:happy #layout:right
* [Wet]
    Hey, at least there'll be rain, right? #character:lila #portrait:neutral #layout:right
    Yeah, but then it'll be muggy tomorrow. #character:ffion #portrait:annoyed #layout:left

-
Whatever it'll be like tomorrow, I'll probably end up staying inside. #character:lila #portrait:neutral #layout:right
What about you? #portrait:happy
I'll do that too, but after I can walk home. #character:ffion #portrait:neutral #layout:left
~ test_variable = "got through the dialogue"
->END