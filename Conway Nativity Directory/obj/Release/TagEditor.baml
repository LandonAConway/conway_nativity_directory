   M S B A M L   `   `   `  ���� T  PConway Nativity Directory, Version=1.2.1.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Conway_Nativity_DirectoryConway_Nativity_Directory  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem )    #Conway_Nativity_Directory.TagEditor   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     9local'clr-namespace:Conway_Nativity_Directory     9system&clr-namespace:System;assembly=mscorlib       J� CommandBindings  )  #System.Windows.Input.CommandBinding  -      Command$ local:CustomCommands.Ok��  -   $  local:CustomCommands.Close�� J� InputBindings ��  �� Command$ local:CustomCommands.Ok��
 �� Key$ Return���� $  local:CustomCommands.Close��$	 Esc�� � 	Resources /   )Conway_Nativity_Directory.DivideConverter   divideBy2Converter   Divisor#   Conway_Nativity_Directory.Tag �� 
TargetType  tagStyle �� Property	 �� Value
 �� Path �� RelativeSource �� AncestorType)   #Conway_Nativity_Directory.TagEditor#   Conway_Nativity_Directory.Tag   RemoveButtonClick   PreviewMouseMove �� Triggers ^� 
Conditions �� Binding �� Value��%s  &	        &	        $ 2Q��� "  ���  �@ RemoveButtonVisibility$	 	Collapsed=���  �@ Brush	 �� $
 	TagsBrush� �� "  ��  �@ MouseOverBrush	 �� $
 TagsMouseOverBrush� �� "  ��  �@ 
Foreground	 �� $
 
Foreground� �� "  ��  �@ ButtonsBrush	 �� $
 TagButtonsBrush� �� "  ��  �@ ButtonsMouseOverBrush	 �� $
 TagButtonsMouseOverBrush� �� "  ;�  Tag_RemoveButtonClick;�  Tag_PreviewMouseMove ^�  ��  �� $
 Editing� �� "   True0���  �@ Brush	 �� $
 EditingTagsBrush� �� "  ��  �@ MouseOverBrush	 �� $
 EditingTagsMouseOverBrush� �� "  ��  �@ 
Foreground	 �� $
 EditingForeground� �� "  ��  �@ RemoveButtonVisibility$	 Visible=�^�  ��  �� $
 Editing� �� "   True��  �� $
 Moving� �� "   True��  �� $
 	IsPressed� �� *Self+ True0���  �@ BorderBrush	 �� $
 MovingTagsBorderBrush� �� "  ��  �@ BorderThickness$	 1q�.���� -   $
��main�� �� Orientation$ 
Horizontal=�.+��� -   $��buttons��$ 
Horizontal=�
������   �� Binding �� Value  tagEditorButtonsStyle��%�  '��       &	      �� " ��.���  � y�$	 	Collapsed=� ��  �� $
 Editing� �� "   True.a���  � y�$	 Visible=��� " F� ^�  ��  �� $
 Editing� �� "  	 False��  �� $
 IsMouseOver� �� *FindAncestor+" ��	 False0���  � ��	 �� $
 ButtonsBrush� �� "  ^�  ��  �� $
 Editing� �� "  	 False��  �� $
 IsMouseOver� �� *FindAncestor+" �� True0���  � ��	 �� $
 ButtonsMouseOverBrush� �� "  ^�  ��  �� $
 Editing� �� "   True��  �� $
 IsMouseOver� �� *FindAncestor+" ��	 False0���  � ��	 �� $
 EditingButtonsBrush� �� "  ^�  ��  �� $
 Editing� �� "   True��  �� $
 IsMouseOver� �� *FindAncestor+" �� True0���  � ��	 �� $"
 EditingButtonsMouseOverBrush� �� "  .+��� -   $
��edit��
������ ���� $
 ActualHeight� �� *Self+._�F� $��4�� �� Stretch$ Uniform=� �� StrokeEndLineCap$ Triangle=�����@B     � � @B 	#��[ ���� " ��.���  � y�$	 	Collapsed=� ^�  ��  �� $
 IsMouseOver� �� ElementName$
 main�� True��  �� $
 Editing� �� "  	 False0���  � y�$	 Visible=��� -   $��confirm��
������ ���� $
 ActualHeight� �� *Self+._�F� $��4��$ Uniform=�"����@B     � � @B �     � 	#��[ �� -   $��cancel��
������ ���� $
 ActualHeight� �� *Self+._�F� $��4��$ Uniform=�+����        �@B @B �@B     �    @B 	#��[ �� -   $��addTag��
������ ���� $
 ActualHeight� �� *Self+._�F� $��4��$ Uniform=�+���� �     � � @B �     � �@B  � 	#��[ �� -	   $��newTag�� �� CornerRadius �� $
 ActualHeight� �� *Self+ �� 	Converter# [  ���� $
 NewTagBrush� �� "  ._��� $ 
Horizontal=�.+��� -
   $��newTagTextBox��
������ $��0q�$	��4,0q����� $
 NewTagForeground� �� "   �� 
CaretBrush �� $
 NewTagForeground� �� "  �� -   $��ConfirmNewTagButton��
������ $	��2,4q����� $
 ActualHeight� �� *Self+��Z� ._�F� $��2��$ Uniform=�$��0q�"����@B     � � @B �     � 	���� " F� ��  �� $
 IsMouseOver�$ ConfirmNewTagButton�� True.a���  � ��	 �� $!
 NewTagButtonsMouseOverBrush� �� "  ��  �� $
 IsMouseOver�$ ConfirmNewTagButton��	 False.a���  � ��	 �� $
 NewTagButtonsBrush� �� "  �� -   $��CancelNewTagButton��
������ $��2,4,4,4q����� $
 ActualHeight� �� *Self+��Z� ._�F� $��2��$ Uniform=�$��0q�+����        �@B @B �@B     �    @B 	���� " F� ��  �� $
 IsMouseOver�$ CancelNewTagButton�� True.a���  � ��	 �� $!
 NewTagButtonsMouseOverBrush� �� "  ��  �� $
 IsMouseOver�$ CancelNewTagButton��	 False.a���  � ��	 �� $
 NewTagButtonsBrush� �� "  ���� " ��.���  � y�$	 	Collapsed=� ^�  ��  �� $
 Editing� �� "   True��  �� $
 Adding� �� "   True0���  � y�$	 Visible=��� -   $
��tags��
������ ���� $
 
TagsSource� �� "  ���� .R�$�  $� Orientation$ 
Horizontal=����� .R�     Loaded 
Tag_Loaded#��[ $��2.5,0q�   Text �� �� -   $��	findRange��$	��100��
������ 