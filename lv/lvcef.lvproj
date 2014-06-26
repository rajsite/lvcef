<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="13008000">
	<Item Name="My Computer" Type="My Computer">
		<Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.tcp.enabled" Type="Bool">false</Property>
		<Property Name="server.tcp.port" Type="Int">0</Property>
		<Property Name="server.tcp.serviceName" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.tcp.serviceName.default" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
		<Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="specify.custom.address" Type="Bool">false</Property>
		<Item Name="Init.vi" Type="VI" URL="../Init.vi"/>
		<Item Name="OnQuery.vi" Type="VI" URL="../OnQuery.vi"/>
		<Item Name="RunJS.vi" Type="VI" URL="../RunJS.vi"/>
		<Item Name="RunJS2.vi" Type="VI" URL="../RunJS2.vi"/>
		<Item Name="Dependencies" Type="Dependencies">
			<Item Name="LVCef.dll" Type="Document" URL="../../dotnet/LVCef/bin/Debug/LVCef.dll"/>
			<Item Name="mscorlib" Type="VI" URL="mscorlib">
				<Property Name="NI.PreserveRelativePath" Type="Bool">true</Property>
			</Item>
			<Item Name="System.Windows.Forms" Type="Document" URL="System.Windows.Forms">
				<Property Name="NI.PreserveRelativePath" Type="Bool">true</Property>
			</Item>
			<Item Name="Xilium.CefGlue.dll" Type="Document" URL="../../dotnet/LVCef/bin/Debug/Xilium.CefGlue.dll"/>
		</Item>
		<Item Name="Build Specifications" Type="Build"/>
	</Item>
</Project>
