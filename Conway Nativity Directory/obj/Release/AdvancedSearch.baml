   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  .    (Conway_Nativity_Directory.AdvancedSearch   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Conway_Nativity_Directory       � Title$  Advanced Search��$	��400��$	��700�� � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .   � Topmost . .���  � 	Resources <   6Conway_Nativity_Directory.BooleanToVisibilityConverter    booleanToVisibilityConverter ' #inverseBooleanToVisibilityConverter   Invert��%%   &	        &	           .  � RowDefinitions �� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $��termsListView��$��
10,10,10,5q���.  ����  �� 
TargetType" ��.���  �� Property �@��
IsSelected	 �� Value	 �� 
 �� Path$
 
IsSelected����� .R�Q� ����  �� Orientation$ 
Horizontal=�.+���  d� 	IsChecked �� $
 	IsEnabled�-   'Conway_Nativity_Directory.CustomTextbox  $	��2,0q�$��Center=����� $

 Text�y��� $
 SearchForText� �� 	Converter# [  .������ "  .���  � ��
	 ����  �� Triggers P�  P� Property �@J�IsMouseOver P� Value .  . ���  � ��$	 0q�P�  �@J�IsMouseOver . . ���  �@��
IsReadOnly	 .  P�  �@ IsKeyboardFocused .  . ���  �@��
IsReadOnly	 . .���� $��Text Disabled��$��Italic$�
�������$	��5,0q�y��� $
 SearchForText�# [ ��  �� CornerRadius$ 5��$��12��$��12��$��1q�
�������$��Center=����� $
 Brush����� " �� ��  �� Binding �� $"
 HighlightWordsInDescriptions� �� RelativeSource ��  �� AncestorType$ local:AdvancedSearchL� �� Value	 False.a���  � y�$	 	Collapsed=���  �� $
 SearchForText�	 False.a���  � y�$	 	Collapsed=�.@��� .+��� .+��� $��Search For Text�� �� $
 SearchForText�Q� $ ��Search In These Categories��y��� $
 SearchForText�# [  .@��� .+��� $��Id�� �� $
 Id��� $��Title�� �� $
 Title��� $��Origin�� �� $
 Origin��� $��Acquired�� �� $
 Acquired��� $
��From�� �� $

 From��� $
��Cost�� �� $

 Cost��� $��Location�� �� $
 Location��� $
��Tags�� �� $

 Tags��� $��Geographical Origins�� �� $
 GeographicalOrigins��� $��Description�� �� $
 Description�Q� $��Search Range��.@��� .+��� $ 
Horizontal=�.+��� $	��Id:�� �� $
 IdRange��� $ 
Horizontal=�$��5,0,0,0q�.+�     NumericOnly . ���� $
 Id1Range��� $��to��$	��5,0q�   . ���� $
 Id2Range����� " �� ��  �� $
 IdRange�	 False.a���  � {�	 .  �� $ 
Horizontal=�.+��� $��	Acquired:�� �� $
 AcquiredRange��� $ 
Horizontal=�$��5,0,0,0q�.+�   . ���� $
 Acquired1Range��� $��to��$	��5,0q�   . ���� $
 Acquired2Range����� " �� ��  �� $
 AcquiredRange�	 False.a���  � {�	 .  �� $ 
Horizontal=�.+��� $��Cost:�� �� $
 	CostRange��� $ 
Horizontal=�$��5,0,0,0q�.+��� $��$��$��0,0,2,0q�   . ���� $
 
Cost1Range��� $��to��$	��5,0q��� $��$��$��0,0,2,0q�   . ���� $
 
Cost2Range����� " �� ��  �� $
 	CostRange�	 False.a���  � {�	 .  Q� $��Other Options��.@��� .+��� $��Case Sensitive�� �� $
 CaseSensitive�y��� $
 SearchForText�# [  �� $��Isolate�� �� $
 Isolate�y��� $
 SearchForText�# [  �� $$��Search For Everything But This�� �� $ 
 SearchForEverythingButThis��� $!��Exclude From Search Results�� �� $
 ExcludeFromSearchResults��� $��1��$
��10,5q�$ 
Horizontal=�.+��� -   $	��100��$��24��$	��Add���� -   $	��100��$��24��$��Remove���� -   $	��100��$��24��$��
Edit Color������ " �� ��  �� $
 	IsChecked� �� ElementName$ highlightCheckBox��	 False.a���  � {�	 .   ��%3   '��       �� " ��.���  � ��$	 0,0,10,0q��� $��2��$
��10,2q�$ 
Horizontal=�.+��� -   $��highlightCheckBox��$%��Highlight Words In Descriptions�� �� $"
 HighlightWordsInDescriptions� �� $ local:AdvancedSearchL��� $��3��$��10,15,10,10q�$��RightToLeft=�$ 
Horizontal=�.+��� -   $��24��$	��100��$��Close���� -   $��24��$	��100��$
��Hide���� -   $��24��$	��100��$��Clear���� -	   $��24��$	��100��$��Search�� ��%3   '��       �� " ��.���  � ��$	 10,0,0,0q�