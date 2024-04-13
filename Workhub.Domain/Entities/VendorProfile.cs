using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workhub.Domain.Entities;
public class VendorProfile
{
    public string Description {get; set;} = string.Empty;
    public ImageDet Image1 {get; set;} = new ImageDet();
    public ImageDet Image2 {get; set;} = new ImageDet();
    public string Instagram {get; set;} = string.Empty;
    
}