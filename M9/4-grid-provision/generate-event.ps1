###################################################################
##  START EVENT HUB CONSUMER subscriber.exe FROM PREVIOUS DEMO   ##
###################################################################

######################################################## 
##  TO GENERATE ACTIVITY REPEAT THE FOLLOWING COMMAND  #
######################################################## 

# Update tag of monitoring RG. Required about 45 to appear in subscriber console
Set-AzResourceGroup -name Demo-AZ204-monitor -Tag @{Code=(Get-Random)}
