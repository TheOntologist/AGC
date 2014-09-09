Thank you for installing Accessible Google Calendar or AGC.
AGC is an application which helps visually disabled people work with Google Calendar. 

On first run AGC will open web browser and ask for permission to change your Calendar data. If you are not logged into 
your Google account you'll first need to do this in your web browser. AGC uses secure OAuth2 authentication, your
password is NOT stored anywhere locally.

AGC has 5 tabs: 
1) Events
2) Create Events
3) Quick Events
4) Settings
5) Sounds 

This readme contains complete description of all tabs interface and functionality.

Events tab

This is the first tab in AGC, pressing Esc button on any other tab will return you here. It is used to view existing
Google calendar events. First control on this tab is a list where you can browse your events using UP and DOWN arrow
keys. Event data is presented as a string which contains following information: 

1) Start time
2) Status (Empty - Confirmed, * - Tentative)
3) Title
4) Content
5) Location
6) End time  

Pressing "Return" will open update dialog for selected event, whereas "Delete" key is used to delete event. In both
cases update or delete dialog is opened for recurring events with 3 options: update or delete single event, update or
delete all events in the series, update or delete all following events. Last option allows you to change events in the 
future, but leave events in the past unchanged. You can also use quick keys "C" and "T" to mark selected event as
Confirmed or Tentative. 

After Events list, there is a series of buttons called "Select Views" which allows you to select events in a specific
period: Today, Tomorrow, This Week, Next Week, This Month and Next Month. Pressing any of these buttons moves focus
back to Events list control.  

Next series of controls is called "Select month period from current month", it is used to select events in a period of 
1 or more months starting from the current month. This series contains 3 controls:

1) List with Months where you select end month, (remember that start month is current month).
2) List with period types: Single month, All months, Intervening months
3) Button to select period, moves focus back to Events list.

This control allows you to view events in several months period. For example, assume that current month is September, 
and you select December as End Month. Now you can get different results depending on selected period type:

a) All months result - events in September, October, November, December
b) Single Month result - events in December
c) Intervening  months result - events in October and November

 Controls which were described above should satisfy most of your requests, but there can be situations when you might 
 want to view events in a specific custom period, for example in the past. Next control "Choose date" allows you to do 
 this. When you press button "Choose date" it opens dialog where you select "Start Date", optionally you can select 
 "End Date" (if end date is not selected, then period is Start Date + 2 years) and optionally search by specific 
 keyword or phrase.

 You can also sort and filter events in the Events List. To do this you need to check "Enable Sort and Filter" and 
 press "Sorting and Filtering" button. This will open new window where you can configure 4 settings:

 1) Enable sorting and sort by Start date, Status, Title, Content, Location or End date in Ascending or Descending 
 order
 2) Enable start time filter to get events which starts only in a specified time interval, for example from 9 to 10 am.
 3) Enable day of week filter to get events which in specified days of week
 4) Enable status filter to get only confirmed or tentative events

 You can also save your sorting and filtering settings to use them as default, but each time you run AGC "Sorting and 
 Filtering" is disabled, so you will need to enable it first to use your saved settings.

The last control on Events tab is a Log Out button. There is no need to log out each time you close AGC, but as you 
log in only once, if you want to use AGC with another Google account you can log out to do this.

Create Event tab

This tab is used Create new events, same UI is used for updating existing events. First of all you can type events 
title, content and location in according text boxes. You can leave any or even all of them blank if you want. Then you 
need to decide whether it is a full day event (you will need to specify only start date), or not (you will need to 
specify start and end date and time).

Next you can mark "Repeat" check box to make it a recurring event, in this case you will need to specify recurring 
rules and recurrence end condition. There are 7 repeat options:

1) Daily
2) Every Weekday (Mon-Fri)
3) Every Mon, Wed, Fri
4) Every Tuesd. and Thurs.
5) Weekly
6) Monthly
7) Yearly

If you choose Daily, Weekly, Monthly or Yearly you can also select interval, for instance "Repeat every 2 Days".
If you choose Weekly you will need to specify weekdays as well, for instance "Repeat on Monday and Tuesday".
If you choose Monthly you will need to specify weather to repeat it by 
a) day of month like "every 1st day of a month"
b) day of week like "every first Friday of a month"

After setting recurring rules you have to specify recurrence end condition, 3 options are available:

1) Ends Never. This option is good if you want it is a rare event, for example someone's Birthday, but it is not 
recommended for frequent events as you might have too many events in the Events list if for large periods.
2) Ends After specified number or events.
3) Ends On specified date

Next you can set reminder to get an email several minutes, hours or days before the event.

Finally you can mark event as Confirmed (this is default) or tentative and save new event (or update existing one).

Quick Events tab

This tab allows you to use Google Calendar Quick Event feature. Full manual how to use it can be found here: 
https://support.google.com/calendar/answer/36604?hl=en

To add quick event you have to type its details in the text box (first control in this tab) and press return or Add 
button which is right next to the text box. You can also save your event details as a template to use it in future.

Next control is a list templates which you can use by pressing return key to substitute event template details to the 
text box where you can change them and add new Quick Event based on selected template. Other controls on this tab 
allows you to edit selected template by changing it's order in the list or removing it from the list. If you want to 
edit template content you can use existing template to add new template and delete the old one. There are seven 
default templates which demonstrate Quick Add syntax functionality and how to use it:

1) 1 hour event next available time, Example: Meeting at 5 pm 
2) full day event on specific date, Example: Day off 2/8
3) several hours event next weekday, Example: Lectures 9:00 - 12:00 next Monday
4) several days event in specified location: Example: Vacation 9/23 - 9/26 in Spain
5) recurring event on weekday for one hour: Example: Meeting at work every Monday at 2pm
6) daily recurring event for 10 times: Example: Lunch every day 1-2 pm for 10 days
7) daily recurring event specified start and end dates, Example: TV Series August 5 daily until August 20

Settings tab

This tab contains section Date-Time Preferences which allows you to configure how events details are presented in the 
event list. There are 10 settings which you can configure:

1) Date Format. Examples: 
	a) day first - Tue 9 Sep
	b) month first - Sep Tue 9

2) Show month. Examples: 
	a) show - Tue 9 Sep
	b) hide - Tue 9

3) Month Format. Examples: 
	a) short name - Sep
	b) name - September
	c) number - 10

4) Time Format. Examples: 
	a) 24-hour 15:00
	b) am/pm 3:00 PM

5) Field Separator between day, month and year. Examples:
	a) space - Tue 9 Sep 2014
	b) _ - Tue 9_Sep_2014
	c) / - Tue 9/Sep/2014
	d) . - Tue 9.Sep.2014

6) Show end time. Allows you to show or hide end data and time of the events.

7) Show year. Allows you to show or hide year in events start and end date.

8) Do not show month for current month events. Allows you to hide month in the event start and and date if events 
happens in current month.

9) Do not show time and end date for full day events. Allows you to show only start date for full days events.

10) Group Events by months using month name as a title. Example of the Event List:
	September
	Event 1
	Event 2
	Event 3
	October
	Event 4
	Event 5
	...

After configuring these settings you need to press Save button. Unlike Sorting and Filtering setting, Date-time 
preferences are always enabled.

Sounds tab.

This tab allows you to use configure sounds in AGC. First of all you can disable or enable sounds. If sounds are 
disabled, than pop up messages are showed when something happens. They are a bit more informative than sounds, but you 
will have to do an extra click to close them every time...

Next you have to select sound type to configure. There are 4 types of sounds in AGC:

1) Success. This sound is played when something was successfully done, for instance new event was added.
2) Neutral. This sound is played in neutral situations, for example when no events were found.
3) Error. This sound is played when something goes wrong. 

IMPORTANT! Normally you shouldn't hear this sound. You can check log file to find what was wrong. However, most 
probably you want be able to resolve this problem on your own. Best thing you can do in this case is to contact 
developer by email (you can find it in Contacts section of this readme) with steps to reproduce this error and copy of 
your log file.

4) Delete. This sound is played when something was deleted.

After you selected sound type, move to the next control which is a list with all available sounds. There you can:
a) Press Return to set selected sound for selected sound type
b) Press Space to play sound
c) Press Delete to delete sound from the list

List of all sounds is common and contains sounds for all types. This means that for instance if you don't like the 
idea of neutral sounds you can set success sound to be played for both sound types.

You cannot delete sounds which are in used. If you try to do this error window will pop up with information for which 
sound type this file is used.

You CAN delete default sounds. If you did it by accident, there is a copy of default sound files in AGC installation 
folder which is probably in your Program Files.

There are also buttons "Set Sound" and "Delete Sound" which do same things as hot keys which where described above.

The last button Add New Sound opens file browser dialog which allows you to add new sounds to the sound list.
Sound file can be in wav or mp3 format. Windows short sounds can be found here:

C:\Windows\Media

Important Files and Folders

AGC stores logs, sound files, and user settings here:
%AppData%\AGC

This readme can be found in your AGC installation folder which by default is:
C:\Program Files (x86)\AGC

Contacts and Credits

AGC is an open source application, it's source code is available on GitHub here:
https://github.com/TheOntologist/AGC

AGC UI is based on original AGCA application developed by Dimitrios Zlitidis.
It's source code is available on Google Code here:
https://code.google.com/p/accessible-front-end-google-calendar/

Current AGC was developed by Nikita Abramovs during his summer vacation work in 2014 in the University of Manchester.
contact: nikita.abramovs@gmail.com

AGC is owned by Professor Robert Stevens who was Nikita Abramovs supervisor.
contact: robert.stevens@manchester.ac.uk