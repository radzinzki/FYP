//
//  ApparelChooserViewController.m
//  iPod+Kinect
//
//  Created by Arnav Kumar on 15/3/14.
//  Copyright (c) 2014 Arnav Kumar. All rights reserved.
//

#import "ApparelChooserViewController.h"
#import "ConnectionManager.h"

@interface ApparelChooserViewController ()

@end

@implementation ApparelChooserViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.title = @"What do you want to try?";
    
    UIBarButtonItem *temporaryBarButtonItem = [[UIBarButtonItem alloc] init];
    temporaryBarButtonItem.title = @"Choose";
    self.navigationItem.backBarButtonItem = temporaryBarButtonItem;
    
    [self addCardViewWithID:[NSNumber numberWithInt:1] Image:[UIImage imageNamed:@"1.png"] ButtonTitle:@"Choose"];
    [self addCardViewWithID:[NSNumber numberWithInt:2] Image:[UIImage imageNamed:@"2.png"] ButtonTitle:@"Choose"];
    [self addCardViewWithID:[NSNumber numberWithInt:3] Image:[UIImage imageNamed:@"3.png"] ButtonTitle:@"Choose"];
    [self addCardViewWithID:[NSNumber numberWithInt:4] Image:[UIImage imageNamed:@"4.png"] ButtonTitle:@"Choose"];
    [self addCardViewWithID:[NSNumber numberWithInt:5] Image:[UIImage imageNamed:@"5.png"] ButtonTitle:@"Choose"];
    [[ConnectionManager sharedManager] sendMessage:[NSString stringWithFormat:@"/apparel_start/"]];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
}

- (void) buttonPressedOnCardView:(TTCardView *)cardView
{
    NSLog(@"Pressed");
    [[ConnectionManager sharedManager] sendMessage:[NSString stringWithFormat:@"/cloth/%@", cardView.ID]];
    [self performSegueWithIdentifier:@"designSegue" sender:cardView.ID];
}

- (void) viewDidDisappear:(BOOL)animated
{
    [[ConnectionManager sharedManager] sendMessage:[NSString stringWithFormat:@"/apparel_stop/"]];
}

- (void) prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender
{
    
}

@end
