   M S B A M L   `   `   `  ����T  PConway Nativity Directory, Version=2.2.3.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      )    #Conway_Nativity_Directory.TagEditor   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Conway_Nativity_Directory     5      9system&clr-namespace:System;assembly=mscorlib     5      |�.   J� CommandBindings  5
      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ local:CustomCommands.Ok��5      6;     5   
   -   5        $  local:CustomCommands.Close��5      6>    J� InputBindings 5      �� 5   
    �� Command$ local:CustomCommands.Ok��6   
 �� Key$ Return��67   �� 5   
   $  local:CustomCommands.Close��6   $	 Esc��6:    � 	Resources 5      /   )Conway_Nativity_Directory.DivideConverter   divideBy2Converter   Divisor#   Conway_Nativity_Directory.Tag �� 
TargetType  tagStyle �� Property	 �� Value
 �� Path �� RelativeSource �� AncestorType)   #Conway_Nativity_Directory.TagEditor#   Conway_Nativity_Directory.Tag   RemoveButtonClick   PreviewMouseMove �� Triggers ^� 
Conditions �� Binding �� Value��%  &	        &	        5   
   $ 2Q�6;   �� 5   
   "  6   �5      �� 5       �@ RemoveButtonVisibility$	 	Collapsed=�6;   �� 5       �@ Brush	 6*   �� $
 	TagsBrush� �� "  �� 5       �@ MouseOverBrush	 63   �� $
 TagsMouseOverBrush� �� "  �� 5       �@ 
Foreground	 6/   �� $
 
Foreground� �� "  �� 5       �@ ButtonsBrush	 61   �� $
 TagButtonsBrush� �� "  �� 5       �@ ButtonsMouseOverBrush	 6:   �� $
 TagButtonsMouseOverBrush� �� "  ;� 5!   8    Tag_RemoveButtonClick;� 5"   7    Tag_PreviewMouseMove 5$      ^� 5%       5&      �� 5'       6$   �� $
 Editing� �� "   True5(   e   0�5*      �� 5+       �@ Brush	 62   �� $
 EditingTagsBrush� �� "  �� 5-       �@ MouseOverBrush	 6;   �� $
 EditingTagsMouseOverBrush� �� "  �� 5/       �@ 
Foreground	 67   �� $
 EditingForeground� �� "  �� 51       �@ RemoveButtonVisibility$	 Visible=�6C   53      ^� 54       55      �� 56       6$   �� $
 Editing� �� "   True57   e   �� 58       6$   �� $
 Moving� �� "   True59   e   �� 5:       6$   �� $
 	IsPressed� �� *Self+ True5;   D   0�5=      �� 5>       �@ BorderBrush	 68   �� $
 MovingTagsBorderBrush� �� "  �� 5@       �@ BorderThickness$	 1q�6<   5B      5D      5E      .��5F      �� -   5        $
��main��5F       �� Orientation$ 
Horizontal=�6   .+�5G   
   �� -   5        $��buttons��5G      $ 
Horizontal=�6$   
������  5H       �� Binding �� Value  tagEditorButtonsStyle��%�  '��       &	 �     �� 5I      " ��6   .�5J      ��  � y�$	 	Collapsed=�63    5K      �� 5L       6&   �� $
 Editing� �� "   True5M   i   .a�5N      ��  � y�$	 Visible=�6;   5O      5Q      �� 5R      " F�6    5S      ^� 5T       5U      �� 5V   "    6,   �� $
 Editing� �� "  	 False5W   u   �� 5X   "    6,   �� $
 IsMouseOver� �� *FindAncestor+" ��	 False5Y   z   0�5[      �� 5\   "    � ��	 6;   �� $
 ButtonsBrush� �� "  5_      ^� 5`       5a      �� 5b   "    6,   �� $
 Editing� �� "  	 False5c   u   �� 5d   "    6,   �� $
 IsMouseOver� �� *FindAncestor+" �� True5e   z   0�5g      �� 5h   "    � ��	 6;   �� $
 ButtonsMouseOverBrush� �� "  5k      ^� 5l       5m      �� 5n   "    6,   �� $
 Editing� �� "   True5o   u   �� 5p   "    6,   �� $
 IsMouseOver� �� *FindAncestor+" ��	 False5q   z   0�5s      �� 5t   "    � ��	 6;   �� $
 EditingButtonsBrush� �� "  5w      ^� 5x       5y      �� 5z   "    6,   �� $
 Editing� �� "   True5{   u   �� 5|   "    6,   �� $
 IsMouseOver� �� *FindAncestor+" �� True5}   z   0�5      �� 5�   "    � ��	 6;   �� $"
 EditingButtonsMouseOverBrush� �� "  5�      5�      5�      .+�5�      �� -   5        $
��edit��5�      
������ ��5�      �� $
 ActualHeight� �� *Self+._�5�      F� $��4��6    �� Stretch$ Uniform=�6+    �� StrokeEndLineCap$ Triangle=�6=   ����@B     � � @B 	#��[ 5�   *   ��5�      �� 5�      " ��6   .�5�      ��  � y�$	 	Collapsed=�67    5�      ^� 5�       5�   "   �� 5�   &    60   �� $
 IsMouseOver� �� ElementName$
 main�� True6g   �� 5�   &    60   �� $
 Editing� �� "  	 False5�   u   0�5�   "   �� 5�   &    � y�$	 Visible=�6C   5�      5�      5�      �� 5�      -   5        $��confirm��5�      
������ ��5�      �� $
 ActualHeight� �� *Self+._�5�      F� $��4��6   $ Uniform=�6+   "����@B     � � @B �     � 	#��[ 5�   0   5�      �� 5�      -   5        $��cancel��5�      
������ ��5�      �� $
 ActualHeight� �� *Self+._�5�      F� $��4��6   $ Uniform=�6+   +����        �@B @B �@B     �    @B 	#��[ 5�   2   5�      �� 5�      -   5        $��addTag��5�      
������ ��5�      �� $
 ActualHeight� �� *Self+._�5�      F� $��4��6   $ Uniform=�6+   +���� �     � � @B �     � �@B  � 	#��[ 5�   8   5�      �� 5�      -	   5        $��newTag��5�       �� CornerRadius 5�      �� $
 ActualHeight� �� *Self+ �� 	Converter# [  ��5�      �� $
 NewTagBrush� �� "  ._�5�      �� $ 
Horizontal=�6   .+�5�      �� -
   5        $��newTagTextBox��5�      
������ $��0q�6L   $	��4,0q�6`   ��5�      �� $
 NewTagForeground� �� "   �� 
CaretBrush 5�      �� $
 NewTagForeground� �� "  �� 5�      -   5        $��ConfirmNewTagButton��5�      
������ $	��2,4q�5�   ]   ��5�   8   �� $
 ActualHeight� �� *Self+��5�      Z� ._�5�      F� $��2��6   $ Uniform=�63   $��0q�6E   "����@B     � � @B �     � 	��5�      �� 5�   "   " F�6(    5�   &   �� 5�   *    66   �� $
 IsMouseOver�$ ConfirmNewTagButton�� True6|   .a�5�   .   ��  � ��	 5�   -   �� $!
 NewTagButtonsMouseOverBrush� �� "  5�   +   �� 5�   *    66   �� $
 IsMouseOver�$ ConfirmNewTagButton��	 False6|   .a�5�   .   ��  � ��	 5�   -   �� $
 NewTagButtonsBrush� �� "  5�   +   5�   #   5�      5�      �� 5�      -   5        $��CancelNewTagButton��5�      
������ $��2,4,4,4q�5�   ]   ��5�   7   �� $
 ActualHeight� �� *Self+��5�      Z� ._�5�      F� $��2��6   $ Uniform=�63   $��0q�6E   +����        �@B @B �@B     �    @B 	��5�      �� 5�   "   " F�6(    5�   &   �� 5�   *    66   �� $
 IsMouseOver�$ CancelNewTagButton�� True6{   .a�5�   .   ��  � ��	 5�   -   �� $!
 NewTagButtonsMouseOverBrush� �� "  5�   +   �� 5�   *    66   �� $
 IsMouseOver�$ CancelNewTagButton��	 False6{   .a�5�   .   ��  � ��	 5�   -   �� $
 NewTagButtonsBrush� �� "  5�   +   5�   #   5�      5�      5�      ��5�      �� 5�      " ��6   .�5�      ��  � y�$	 	Collapsed=�67    5�      ^� 5�       5�   "   �� 5�   &    60   �� $
 Editing� �� "   True5�   q   �� 5�   &    60   �� $
 Adding� �� "   True5�   q   0�5�   "   �� 5�   &    � y�$	 Visible=�6C   5�      5�      5�      5�      �� 5�   
   -   5        $
��tags��5�      
������ ��5�      �� $
 
TagsSource� �� "  ��5     �� 5     .R�5     $�  $� Orientation$ 
Horizontal=�6-   5     ��5	     �� 5
     .R�5          Loaded 
Tag_Loaded6b   #��[ 61   $��2.5,0q�6S      Text 6    �� 6b   5     5     �� 5  
   -   5        $��	findRange��5     $	��100��6"   
������ 6.   5     5     