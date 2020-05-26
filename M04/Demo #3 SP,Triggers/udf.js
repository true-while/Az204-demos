function Match(input, pattern) 
{ 
    //expect correct patter for regex and return TRUE if it is matched to input
    return input.match(pattern) !== null;
};
