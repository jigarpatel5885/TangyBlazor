window.ShowToastr=(type,message)=> {

    if (type === "success") {
        toastr.success(message, 'Operation Successful', { timeOut: 5000 });

    }
    if (type === "error") {

        toastr.error(message, 'Operation Failed', { timeOut: 5000 });
    }
}

window.ShowSwAlert = (type, message) => {
    //if (type === "success") {
    //    swal({          
    //        text: message,
    //        icon: "success"
    //    });
    //}

    //if (type === "error") {
    //    swal({
    //        text: message,
    //        icon: "error"
    //    });
    //}

    if (type === "success") {
        Swal.fire(
            'Success Notification!',
            message,
            'success'
        )
    }
    if (type === "error") {
        Swal.fire(
            'Error Notification!',
            message,
            'error'
        )
    }
}



function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}

function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}