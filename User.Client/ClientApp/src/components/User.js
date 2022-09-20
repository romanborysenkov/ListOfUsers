import React, { useState, useEffect } from "react";

const defaluImageSrc = '/img/profile.png'
const initialFieldValues = {
     userId: 0,
     userName: '',
     email: '',
     profession: '',
     imageName:'',
     imageSrc: defaluImageSrc,
     imageFile: null
}


export default function User(props) {
    const { addOrEdit, recordForEdit } = props

    const [values, setValues] = useState(initialFieldValues)
    const [errors, setErrors] = useState({})

    useEffect(() => {
        if(recordForEdit!=null)
            setValues(recordForEdit);
    }, [recordForEdit])

    const handleInputChange = e => {
        const { name, value } = e.target;
        setValues({
            ...values,
            [name]: value
        })
    }

    const showPreview = e => {
        if (e.target.files && e.target.files[0]) {
            let imageFile = e.target.files[0];
            const reader = new FileReader();
            reader.onload = x => {
                setValues({
                    ...values,
                    imageFile,
                    imageSrc: x.target.result
                })
            }
            reader.readAsDataURL(imageFile)
        }
        else {
            setValues({
                ...values,
                imageFile: null,
                imageSrc: defaluImageSrc
            })
        }
    }
    const validate = () => {
        let temp = {}
        temp.userName = values.userName == "" ? false : true;
        temp.imageSrc = values.imageSrc == defaluImageSrc ? false : true;
        setErrors(temp)
        return Object.values(temp).every(x => x == true)
    }

    const resetForm = () => {
        setValues(initialFieldValues)
        document.getElementById('image-uploader').value = null;
        setErrors({})
    }

    const handleFormSubmit = e => {
        e.preventDefault()
        if (validate()) {
            const formData = new FormData()
            formData.append('userId', values.userId)
            formData.append('userName', values.userName)
            formData.append('email', values.email)
            formData.append('profession', values.profession)
            formData.append('imageName', values.imageName)
            formData.append('imageFile', values.imageFile)
            addOrEdit(formData, resetForm)
        }
    }

    const applyErrorClass = field => ((field in errors && errors[field] == false) ? ' invalid-field' : '')

    return (
        <>
            <div className="conainer text-center">
                <p className="lead">An User</p>
                </div>
            <form autoComplete="off" noValidate onSubmit={handleFormSubmit}>
                <div className="card">
                    <img src={values.imageSrc} className="card-img-top" />
                    <div className="card-body">
                        <div className="form-group">
                            <input type="file" accept="image/*" className={"form-controle-file"+applyErrorClass('imageSrc')}
                                onChange={showPreview} id="image-uploader" />
                        </div>
                        <div className="form-group">
                            <input type="text"  className={"form-control" + applyErrorClass('userName')} placeholder="User Name" name="userName"
                                value={values.userName} onChange={handleInputChange} />
                        </div>
                        <div className="form-group">
                            <input type="text"  className="form-control" placeholder="Email " name="email"
                                value={values.email}
                                onChange={handleInputChange} />
                        </div>
                        <div className="form-group">
                            <input type="text" className="form-control" placeholder="Profession " name="profession"
                                value={values.profession}
                                onChange={handleInputChange} />
                        </div>
                        <div className="form-group text-center">
                            <button type="submit" className="btn btn-light">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
        </>
    )
}