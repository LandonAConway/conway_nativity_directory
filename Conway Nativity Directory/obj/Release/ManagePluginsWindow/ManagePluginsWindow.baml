   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  3    -Conway_Nativity_Directory.ManagePluginsWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Conway_Nativity_Directory       � Title$  Manage Plugins��$	��500��$	��900�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=�.���  � RowDefinitions �� $��* ��� $
��Auto �.+��� -    ��Name$ installedItemsListView�� �� SelectionMode$ Single=�$��
10,10,10,5q�����  �� 
TargetType" �� �� Triggers P�  P� Property �@��
IsSelected	 P� Value	 . . ��� 
 �� Property
 � �� �� Value �	#4C008BFF�� 
 � �� �	#FF4689FF�� 
 � �� �	#7F008BFFP�  �@J�IsMouseOver	 . . ��� 
 � �� �	#19008BFF�� 
 � �� �	#4C008BFF.��� 
 � �� ��  �� 
TargetType" ��.R���  �� Padding# z��#��z��#��z��#��z�� J� SnapsToDevicePixels . ._��  �� $��* ��� $
��Auto �.+��� $
��Left=�����  �� Path$ Title� �� RelativeSource ��  �� AncestorType3   -Conway_Nativity_Directory.InstalledPluginItem"  ��Z� ���� " ��.��� 
 �@d�	IsChecked �� *PluginIsEnabled+ �� "  �� $��1��$��0,5,0,0q�._���  �� Orientation$ 
Horizontal=�.+���  ��Name$ uninstallBtn��-   $��	Uninstall��$��24��$	��100������ " ��.��� 
 � y�$ 	Collapsed=� ��  �� Binding �� $ 
IsSelected� �� "   �� Value Truea��� 
 � y�$ 	Collapsed=��� 
 � ��$ 0,0,0,5q��� 
 �@��
IsSelected �� *
IsSelected+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�� $��
10,5,10,10q�$��1��$��2��.+��� $ 
Horizontal=�$��RightToLeft=�.+��� -   $ closeBtn��$��Close��$	��120��$��24��