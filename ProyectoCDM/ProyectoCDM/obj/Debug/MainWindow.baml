   M S B A M L   `   `   `  ����F  BProyectoCDM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ProyectoCDMProyectoCDM  5   	   C&clr-namespace:ProyectoCDM.MVM.VieModelProyectoCDM.MVM.VieModel  5   
       ProyectoCDM.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ProyectoCDM     5   	   ;vieModel&clr-namespace:ProyectoCDM.MVM.VieModel     5   
   $	��600��5	   	   $	��920��6     � WindowStyle$
  None=�5
   	    � 
ResizeMode$ NoResize=�5   	   
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   #    � DataContext 5      ,  &ProyectoCDM.MVM.VieModel.MainViewModel  5   
   .��5      �� ���#272537 �� CornerRadius$ 20��5      $��0,0,-60,-75q�6   ._�5   
   � $��-10,10,-10,-10q�6    � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       � RowDefinitions 5      �� 5      $��75 �6    �� 5      .+�5      �� $	��DCM��6   $��Center=�5      $
��Left=�5       
�������$��22(�5"      $��20,0,0,0q�5#      �� 5'      $��1��6   .+�5)      	� $
��Home��6   $��50��5*      
�������$��14(�5,         MenuButtonTheme#��[  5-       d� 	IsChecked$
 TrueY�5.      ��5/      �� *HomeViewCommmand+	� 50      $��Pet Registration��6   $��50��51      
�������$��14(�53      #��[  54      ��55      �� *PetRegistroViewCommmand+	� 56      $��Home Registration ��6   $��50��57      
�������$��14(�59      #��[  5:      ��5;      �� *HomeRegisterViewCommmand+	� 5<      $��View Security ��6   $��50��5=      
�������$��14(�5?      #��[  5@      ��5A      �� *ViewSecurityViewCommmand+	� 5B      $��Move Detection ��6   $��50��5C      
�������$��14(�5E      #��[  5F      ��5G      �� *MoveDetectionVielCommmand+5I      �� 5J      $��1��6   $��1��5K      $��10q�5L      ��5M      �� *CurrentView+�� 5N      -   5        $��x��5N      $��1��5O      $
��Left=�5P      $��24��5Q      $��	662,0,0,0q�5R      $	��Top=�5S      $��31��5T      6    �� 5U      -   5        $��-��5U      $��1��5V      $
��Left=�5W      $��24��5X      $��	625,0,0,0q�5Y      $	��Top=�5Z      $��32��5[      6    5\      5_      5a      