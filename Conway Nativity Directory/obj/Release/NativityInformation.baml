   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  3    -Conway_Nativity_Directory.NativityInformation   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     9local'clr-namespace:Conway_Nativity_Directory     ����   �� 
TargetType"  >�.���  �� Property � �� �� Value$ Stretch=���  � ��$ Stretch=� � 	Resources    imageContextMenu��%�  &	        �� .@�m� $��embeddMenuItem��-   $��Embedd��m� $��relinkMenuItem��-   $��Relink��m� $��Rotation��.@�m� $��rotateRightMenuItem��-   $��Rotate Right��m� $��rotateLeftMenuItem��-   $��Rotate Left��m� $��refreshMenuItem��-   $��Refresh���� m� $��showInFileExplorer��-   $��Show In File Explorer��m� $��openWithMenuItem��-   $��	Open With��.@�m� $��openWithBrowseMenuItem��-   $��	Browse...��.���  � RowDefinitions �� $
��Auto ��� �� .+��� $��0��$��
10,10,10,5q� �� Orientation$ 
Horizontal=�.+��� $
��Id: ���� -	   $��idTextBlock���� $��Title: ��$��10,0,0,0q��� -
   $��titleTextBlock���� -   $��imageBorder��$��1��$
��10,5q�
������ ._��  �� �� $
��Auto �.+��� -   $��1q�
�������._��� -   $��image�� � ContextMenu# �   �� -   $��chooseImageButton��$��1��$��24��$��Choose������ "  ��.���  � y�$ 	Collapsed=� �� Triggers ^�  ^� 
Conditions �� 	 �� Binding	 �� 
 �� Path$
 ShowingImage� �� RelativeSource ��  �� AncestorType3   -Conway_Nativity_Directory.NativityInformation"   �� Value	 False�� 	 �� $
 IsMouseOver� �� ElementName$ imageBorder�� True0���  � y�$ Visible=��� -   $��clearImageButton��$��1��$��24��$��Clear������ "  ��.���  � y�$ 	Collapsed=� ^�  �� 	 �� $
 ShowingImage� �� "   True�� 	 �� $
 IsMouseOver�$ imageBorder�� True0���  � y�$ Visible=��� -   $��descriptionBorder��$��2��$��
10,5,10,10q�._��  �� �� $
��Auto �.+�1   +Conway_Nativity_Directory.CustomRichTextBox  -     Name$ descriptionRichTextBox�� �� 
IsReadOnly .  �� AcceptsReturn .  �� 
AcceptsTab . ���� $
 ActualWidth�$ descriptionBorder��. �/�  /� 	PageWidth �� $
 ActualWidth�$ descriptionRichTextBox�� �� Mode$ OneWay=����� "    ��  �� Binding �� $
 EditingDescription� �� "   �� Value True.a���  � y�$ 	Collapsed=��� -   $��descriptionTextBox�� �� TextWrapping$
 Wrap=� .  . ���� $
 Description� �� "  .������ "  ��.���  � y�$ 	Collapsed=� ��  �� $
 EditingDescription� �� "   True.a���  � y�$ Visible=���  � ��$ 0q�.���� ���� "  �� ��  �� $
 EditingDescription� �� "   True.a���  � �� �	#ffffa000��  � ��$ 1q��� -   $��editDescriptionButton��$��1��$��24��$
��Edit������ "  ��.���  � y�$ 	Collapsed=� ^�  �� 	 �� $
 EditingDescription� �� "  	 False�� 	 �� $
 IsMouseOver�$ descriptionBorder�� True0���  � y�$ Visible=��� -   $��saveDescriptionButton��$��1��$��24��$
��Save������ "  ��.���  � y�$ 	Collapsed=� ��  �� $
 EditingDescription� �� "   True.a���  � y�$ Visible=�