function EnsureHaveCrew() {
  var collection = getContext().getCollection();
  var request = getContext().getRequest();
  var docToCreate = request.getBody();
  
    // Reject documents Apollo from launch without Crew on board.
  if ( docToCreate["Crew"] != undefined && docToCreate["Crew"]["Members"] != undefined && docToCreate["Crew"]["Members"].length > 0) {
    getContext().getRequest().setBody(docToCreate);
  }
  else
  {
    throw new Error('NO LAUNCH WITHOUT CREW MEMBERS!');
  }
}
