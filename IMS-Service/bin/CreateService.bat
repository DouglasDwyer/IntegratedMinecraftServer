sc create IMS start=auto binpath="C:\Program Files (x86)\IMS\IMS-Service.exe -run"
sc failure IMS actions= restart/30000/restart/30000/""/30000 reset= 43200