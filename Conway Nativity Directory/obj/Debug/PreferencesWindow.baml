   M S B A M L   `   `   `  ����T  PConway Nativity Directory, Version=2.2.3.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  5   	   1    +Conway_Nativity_Directory.PreferencesWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:Conway_Nativity_Directory     5   	   -   5          � Title$  Preferences��5   	   $	��700��6   $
��1000��6*    � WindowStartupLocation$ CenterScreen=�67    � 
ResizeMode$ NoResize=�6\    � ShowInTaskbar .   � Topmost .  � 	Resources 5      8   2Conway_Nativity_Directory.ShowPreferencesConverter   showPreferences��%   &	          5   
   6)   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   R� -   5        $��preferencesTreeView��5      $��0��6.   $��3��6>   $��	10,10,5,5q�6O   ��5      �� 5       �� 
TargetType" Q�6   .�5      �� 	 �� Property	 � ��
 �� Value
 6/   ��  �� Path$ Title��� 5      	 �@Q�
IsExpanded
 63   �� $ 
IsExpanded��� 5      	 �@Q�
IsSelected
 63   �� $ 
IsSelected� �� Triggers 5       �� 5!       �� Binding 6&   �� $ Visible� �� Value	 False6G   .a�5"      �� 	 � y�$
 	Collapsed=�6;   5#      �� 5$       6&   �� $ Items� �� 	Converter# [   True6q   .a�5%      �� 	 � ��
 6<   �� $ Items�5&      5(      5*      �� 5+   
   $��1��6   $��0��6!   $��	5,10,10,5q�6.   ._�5,      �� -   5        $��titleTextBlock��5,      $��Center=�6.   $��Center=�6K   $��16(�6f   5-      �� 5.   
   #��Z�6   $��1q�6I    �� CornerRadius$ 5��6]   $��1��5/      $��1��6!   $��5,5,10,5q�6.   ._�50      �� -   5        $��uiContentControl��50      $��10q�65   51      �� 52   
   -   5        $ ��effectiveImmediatelyBorder��52      $��5,5,10,5q�63   $��1��6E   $��2��6U   $y�	Collapsed=�6b   ._�53      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�6O   .+�54      �� -   5        $��warningIcon��54      $��32��6+   $��32��66   $��0,0,10,0q�6B   �� 55      
���  ��$��Center=�6-   $V��PSome changes will take effect the next time you start Conway Nativity Directory.��56      57      58      �� 59   
   $��0��6   $��3��6%   $��	10,5,5,10q�62   $ 
Horizontal=�6E   .+�5:      �� -   5        $��resetButton��5:      $��Reset��6(   $	��100��68   $��24��6D   5;      �� 5<      -   5        $��resetAllButton��5<      $��	Reset All��6+   $	��100��6?   $��24��6K   $��10,0,0,0q�6W   5=      5>      �� 5?   
   $��1��6   $��3��6%   $��	5,5,10,10q�62   $ 
Horizontal=�6E   $��RightToLeft=�6^   .+�5@      �� -	   5        $��okButton��5@      $��Ok��6%   $	��100��62   $��24��6>   5A      �� 5B      -
   5        $��applyButton��5B      $��Apply��6(   $	��100��68   $��24��6D   $��10,0,0,0q�6P   5C      �� 5D      -   5        $��cancelButton��5D      $��Cancel��6)   $	��100��6:   $��24��6F   $��10,0,0,0q�6R   5E      5F      5G      5H      