#Connect-AzAccount

New-AzResourceGroup -Name Demo2 -Location EastUS

New-AzVm `
    -ResourceGroupName "Demo2" `
    -Name "myVM" `
    -Location "East US" `
    -VirtualNetworkName "myVnet" `
    -SubnetName "mySubnet" `
    -SecurityGroupName "myNetworkSecurityGroup" `
    -PublicIpAddressName "myPublicIpAddress" `
    -OpenPorts 80,3389

$ip = Get-AzPublicIpAddress -ResourceGroupName "Demo2" | Select "IpAddress“

mstsc /v:($ip.IpAddress)

#Install-WindowsFeature -name Web-Server –IncludeManagementTools
