   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  1    +Conway_Nativity_Directory.PreferencesWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Conway_Nativity_Directory     -     � Title$  Preferences��$	��700��$
��1000�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � Topmost .  � 	Resources 8   2Conway_Nativity_Directory.ShowPreferencesConverter   showPreferences��%   &	          .���  � ColumnDefinitions �� $	��300 ���  � RowDefinitions �� $
��Auto ��� �� $
��Auto ��� $
��Auto �.+�R� -   $��preferencesTreeView��$��0��$��3��$��	10,10,5,5q�����  �� 
TargetType" Q�.��� 	 �� Property	 � ��
 �� Value
 ��  �� Path$ Title��� 	 �@Q�
IsExpanded
 �� $ 
IsExpanded��� 	 �@Q�
IsSelected
 �� $ 
IsSelected� �� Triggers ��  �� Binding �� $ Visible� �� Value	 False.a��� 	 � y�$
 	Collapsed=���  �� $ Items� �� 	Converter# [   True.a��� 	 � ��
 �� $ Items��� $��1��$��0��$��	5,10,10,5q�._��� -   $��titleTextBlock��$��Center=�$��Center=�$��16(��� #��Z�$��1q� �� CornerRadius$ 5��$��1��$��1��$��5,5,10,5q�._��� -   $��uiContentControl��$��10q��� -   $ ��effectiveImmediatelyBorder��$��5,5,10,5q�$��1��$��2��$y�	Collapsed=�._���  �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� -   $��warningIcon��$��32��$��32��$��0,0,10,0q��� 
���  ��$��Center=�$V��PSome changes will take effect the next time you start Conway Nativity Directory.���� $��0��$��3��$��	10,5,5,10q�$ 
Horizontal=�.+��� -   $��resetButton��$��Reset��$	��100��$��24���� -   $��resetAllButton��$��	Reset All��$	��100��$��24��$��10,0,0,0q��� $��1��$��3��$��	5,5,10,10q�$ 
Horizontal=�$��RightToLeft=�.+��� -	   $��okButton��$��Ok��$	��100��$��24���� -
   $��applyButton��$��Apply��$	��100��$��24��$��10,0,0,0q��� -   $��cancelButton��$��Cancel��$	��100��$��24��$��10,0,0,0q�